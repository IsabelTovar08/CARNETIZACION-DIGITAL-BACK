using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Business.Interfaces.Auth;
using Business.Interfaces.Logging;
using Business.Interfaces.Organizational.Assignment;
using Business.Interfaces.Security;
using Data.Interfases.Transaction;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Specifics;
using Microsoft.Extensions.Logging;
using Utilities.Helpers.Excel;
using Utilities.Helpers.Images;

namespace Business.Services.Excel
{
    /// <summary>
    /// Servicio que procesa la carga masiva de personas desde un archivo Excel.
    /// </summary>
    public class ExcelBulkImporter : IExcelBulkImporter
    {
        private readonly ILogger _logger;
        private readonly IExcelPersonParser _parser;
        private readonly IPersonBusiness _personBusiness;
        private readonly IPersonDivisionProfileBusiness _pdpBusiness;
        private readonly ICardBusiness _cardBusiness;
        private readonly IUnitOfWork _uow;
        private readonly IImportHistoryBusiness _history;
        private readonly ICurrentUser _currentUser;
        private readonly IExcelReaderHelper _excel;

        public ExcelBulkImporter(
            ILogger<ExcelBulkImporter> logger,
            IExcelPersonParser parser,
            IPersonBusiness personBusiness,
            IPersonDivisionProfileBusiness pdpBusiness,
            ICardBusiness cardBusiness,
            IUnitOfWork uow,
            IImportHistoryBusiness history,
            ICurrentUser currentUser,
            IExcelReaderHelper excelReaderHelper)
        {
            _logger = logger;
            _parser = parser;
            _personBusiness = personBusiness;
            _pdpBusiness = pdpBusiness;
            _cardBusiness = cardBusiness;
            _uow = uow;
            _history = history;
            _currentUser = currentUser;
            _excel = excelReaderHelper;
        }

        /// <summary>
        /// Procesa el archivo Excel, crea las entidades correspondientes y devuelve el resultado.
        /// </summary>
        public async Task<BulkImportResultDto> ImportAsync(Stream excelStream, ImportContextCard ctx)
        {
            var parsed = await _parser.ParseAsync(excelStream);
            var result = new BulkImportResultDto { TotalRows = parsed.Count };
            var tableRows = new List<ImportBatchRowTableDto>();

            // Metadatos de la operación
            var fileName = _excel.GetFileName(excelStream);
            var startedBy = _currentUser.UserName ?? _currentUser.UserIdRaw;

            var ctxJson = JsonSerializer.Serialize(new
            {
                ctx.OrganizationId,
                ctx.OrganizationCode,
                ctx.OrganizationalUnitId,
                ctx.OrganizationalUnitCode,
                ctx.InternalDivisionId,
                ctx.InternalDivisionCode,
                ctx.ProfileId,
                ctx.ValidFrom,
                ctx.ValidTo
            });

            var batchId = await _history.StartBatchAsync(new ImportBatchStartDto
            {
                Source = "Excel",
                FileName = fileName,
                StartedBy = startedBy,
                TotalRows = parsed.Count,
                ContextJson = ctxJson
            });

            foreach (var row in parsed)
            {
                var rowRes = new BulkRowResult { RowNumber = row.RowNumber, UpdatedPhoto = false };
                int? personId = null;
                int? pdpId = null;
                int? cardId = null;
                PersonRegistrerDto? personCreated = null;

                await _uow.BeginTransactionAsync();

                try
                {
                    // === STEP 1: Crear persona/usuario ===
                    var registrer = new PersonRegistrer
                    {
                        Person = row.Person,
                        User = new UserDtoRequest { UserName = row.Person.Email, Password = row.TempPassword ?? "ChangeMe.123" }
                    };

                    personCreated = await _personBusiness.SavePersonAndUser(registrer);
                    var personDto = personCreated.Person;
                    if (personDto == null || personDto.Id <= 0)
                        throw new InvalidOperationException("No fue posible crear la persona.");

                    personId = personDto.Id;
                    rowRes.EmailSent = personCreated.EmailSent;

                    // === STEP 2: Vincular PDP ===
                    var pdpSaved = await _pdpBusiness.Save(new PersonDivisionProfileDtoRequest
                    {
                        PersonId = personId.Value,
                        InternalDivisionId = ctx.InternalDivisionId,
                        ProfileId = ctx.ProfileId,
                        isCurrentlySelected = true,
                    });
                    pdpId = pdpSaved.Id;

                    // === STEP 3: Subir/actualizar foto ===
                    if (row.PhotoBytes is { Length: > 0 })
                    {
                        var (ext, contentType) = ImageFormatValidator.EnsureSupported(row.PhotoBytes, row.PhotoExtension);
                        using var ms = new MemoryStream(row.PhotoBytes);
                        await _personBusiness.UpsertPersonPhotoAsync(personId.Value, ms, contentType, $"excel-upload{ext}");
                        rowRes.UpdatedPhoto = true;
                    }

                    // === STEP 4: Crear carnet ===
                    var card = await _cardBusiness.Save(new CardDtoRequest
                    {
                        CreationDate = ctx.ValidFrom,
                        ExpirationDate = ctx.ValidTo,
                        PersonDivissionProfileId = pdpId.Value,
                        StatusId = 1,
                        CardTemplateId = 1
                    });
                    cardId = card.Id;

                    await _uow.CommitAsync();

                    rowRes.Success = true;
                    rowRes.Created = true;
                    rowRes.Message = "Importación exitosa.";
                    result.SuccessCount++;

                    // Armar DTO para tabla
                    tableRows.Add(new ImportBatchRowTableDto
                    {
                        RowNumber = row.RowNumber,
                        Photo = personDto.PhotoUrl ?? "/images/default-avatar.png",
                        Name = $"{personDto.FirstName} {personDto.LastName}",
                        Org = ctx.OrganizationalUnitCode ?? "N/A",
                        Division = ctx.InternalDivisionCode ?? "N/A",
                        State = "Activo",
                        IsDeleted = false
                    });
                }
                catch (Exception ex)
                {
                    await _uow.RollbackAsync();
                    rowRes.Success = false;
                    rowRes.Created = false;
                    rowRes.Message = Innermost(ex);
                    result.ErrorCount++;
                    _logger.LogWarning(ex, "Error importando fila {Row}", row.RowNumber);

                    // DTO para tabla con error
                    tableRows.Add(new ImportBatchRowTableDto
                    {
                        RowNumber = row.RowNumber,
                        Photo = null,
                        Name = row.Person.FirstName ?? "Desconocido",
                        Org = ctx.OrganizationalUnitCode ?? "N/A",
                        Division = ctx.InternalDivisionCode ?? "N/A",
                        State = "Error",
                        IsDeleted = false
                    });
                }
                finally
                {
                    await _history.AppendRowAsync(new ImportBatchRowDto
                    {
                        ImportBatchId = batchId,
                        RowNumber = row.RowNumber,
                        Success = rowRes.Success,
                        Message = rowRes.Message,
                        PersonId = personId,
                        PersonDivisionProfileId = pdpId,
                        CardId = cardId,
                        UpdatedPhoto = rowRes.UpdatedPhoto
                    });

                    result.Rows.Add(rowRes);
                }
            }

            await _history.CompleteBatchAsync(new ImportBatchCompleteDto
            {
                ImportBatchId = batchId,
                SuccessCount = result.SuccessCount,
                ErrorCount = result.ErrorCount
            });

            result.TableRows = tableRows;
            return result;
        }

        /// <summary>
        /// Obtiene el mensaje más interno de una excepción.
        /// </summary>
        private static string Innermost(Exception ex)
        {
            while (ex.InnerException != null) ex = ex.InnerException;
            return ex.Message;
        }
    }
}

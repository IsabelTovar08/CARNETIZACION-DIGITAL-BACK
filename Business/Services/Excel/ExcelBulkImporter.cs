using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Business.Interfaces.Auth;
using Business.Interfaces.Logging;
using Business.Interfaces.Notifications;
using Business.Interfaces.Operational;
using Business.Interfaces.Organizational.Assignment;
using Business.Interfaces.Security;
using Business.Interfases.Storage;
using Business.Services.Cards;
using Data.Interfases.Transaction;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Specifics;
using Entity.DTOs.Specifics.Cards;
using Entity.Enums.Specifics;
using Microsoft.Extensions.Logging;
using Utilities.Helpers.Excel;
using Utilities.Helpers.Images;

namespace Business.Services.Excel
{
    /// <summary>
    /// Servicio que procesa la carga masiva de personas desde un archivo Excel
    /// y genera automáticamente los carnets en formato PDF usando el template indicado.
    /// </summary>
    public class ExcelBulkImporter : IExcelBulkImporter
    {
        private readonly ILogger _logger;
        private readonly IExcelPersonParser _parser;
        private readonly IPersonBusiness _personBusiness;
        private readonly IIssuedCardBusiness _pdpBusiness;
        private readonly ICardBusiness _cardBusiness;
        private readonly ICardTemplateBusiness _templateBusiness;
        private readonly IUnitOfWork _uow;
        private readonly IImportHistoryBusiness _history;
        private readonly ICurrentUser _currentUser;
        private readonly IExcelReaderHelper _excel;
        private readonly INotificationBusiness _notificationsBusiness;
        private readonly ICardPdfService _cardPdfService;
        private readonly IFileStorageService _storageService;

        public ExcelBulkImporter(
            ILogger<ExcelBulkImporter> logger,
            IExcelPersonParser parser,
            IPersonBusiness personBusiness,
            IIssuedCardBusiness pdpBusiness,
            ICardBusiness cardBusiness,
            ICardTemplateBusiness templateBusiness,
            IUnitOfWork uow,
            IImportHistoryBusiness history,
            ICurrentUser currentUser,
            IExcelReaderHelper excelReaderHelper,
            INotificationBusiness notificationsBusiness,
            ICardPdfService cardPdfService,
            IFileStorageService storageService
        )
        {
            _logger = logger;
            _parser = parser;
            _personBusiness = personBusiness;
            _pdpBusiness = pdpBusiness;
            _cardBusiness = cardBusiness;
            _templateBusiness = templateBusiness;
            _uow = uow;
            _history = history;
            _currentUser = currentUser;
            _excel = excelReaderHelper;
            _notificationsBusiness = notificationsBusiness;
            _cardPdfService = cardPdfService;
            _storageService = storageService;
        }

        /// <summary>
        /// Procesa el archivo Excel, crea las entidades correspondientes,
        /// genera PDFs de los carnets y devuelve el resultado.
        /// </summary>
        public async Task<BulkImportResultDto> ImportAsync(Stream excelStream, ImportContextCard ctx)
        {
            var parsed = await _parser.ParseAsync(excelStream);
            var result = new BulkImportResultDto { TotalRows = parsed.Count };
            var tableRows = new List<ImportBatchRowTableDto>();

            // Metadatos
            var fileName = _excel.GetFileName(excelStream);
            int startedBy = _currentUser.UserId;

            // Crear la configuración base (Card) — solo una vez
            var cardConfig = await _cardBusiness.Save(new CardConfigurationDtoRequest
            {
                CreationDate = ctx.ValidFrom,
                ExpirationDate = ctx.ValidTo,
                CardTemplateId = ctx.CardTemplateId,
                StatusId = 1,
                SheduleId = 1
            });

            var cardConfigId = cardConfig.Id;
            var template = await _templateBusiness.GetById(ctx.CardTemplateId);

            // Guardar registro del batch
            var ctxJson = JsonSerializer.Serialize(new
            {
                ctx.OrganizationId,
                ctx.OrganizationCode,
                ctx.OrganizationalUnitId,
                ctx.OrganizationalUnitCode,
                ctx.InternalDivisionId,
                ctx.InternalDivisionCode,
                ctx.ProfileId,
                ctx.CardTemplateId,
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

            // Procesar cada fila/persona
            foreach (var row in parsed)
            {
                var rowRes = new BulkRowResult { RowNumber = row.RowNumber, UpdatedPhoto = false };
                int? personId = null;
                int? pdpId = null;
                PersonRegistrerDto? personCreated = null;

                await _uow.BeginTransactionAsync();

                try
                {
                    // === STEP 1: Crear persona/usuario ===
                    var registrer = new PersonRegistrer
                    {
                        Person = row.Person,
                        User = new UserDtoRequest
                        {
                            UserName = row.Person.Email,
                            Password = row.TempPassword ?? "ChangeMe.123"
                        }
                    };

                    personCreated = await _personBusiness.SavePersonAndUser(registrer);
                    var personDto = personCreated.Person;
                    if (personDto == null || personDto.Id <= 0)
                        throw new InvalidOperationException("No fue posible crear la persona.");

                    personId = personDto.Id;
                    rowRes.EmailSent = personCreated.EmailSent;

                    // === STEP 2: Vincular PDP ===
                    var pdpSaved = await _pdpBusiness.Save(new IssuedCardDtoRequest
                    {
                        PersonId = personId.Value,
                        InternalDivisionId = ctx.InternalDivisionId,
                        ProfileId = ctx.ProfileId,
                        isCurrentlySelected = true,
                        CardId = cardConfigId,
                        StatusId = 1
                    });
                    pdpId = pdpSaved.Id;

                    //// === STEP 3: Subir foto ===
                    //if (row.PhotoBytes is { Length: > 0 })
                    //{
                    //    var (ext, contentType) = ImageFormatValidator.EnsureSupported(row.PhotoBytes, row.PhotoExtension);
                    //    using var ms = new MemoryStream(row.PhotoBytes);
                    //    await _personBusiness.UpsertPersonPhotoAsync(personId.Value, ms, contentType, $"excel-upload{ext}");
                    //    rowRes.UpdatedPhoto = true;
                    //}

                    // === STEP 4: Generar PDF del carnet individual ===
                    try
                    {
                        var userData = new CardUserData
                        {
                            Name = $"{row.Person.FirstName} {row.Person.LastName}",
                            Email = row.Person.Email,
                            PhoneNumber = row.Person.Phone ?? "",
                            CardId = cardConfigId.ToString(),
                            Profile = ctx.ProfileId.ToString(),
                            CategoryArea = ctx.InternalDivisionCode ?? "",
                            CompanyName = ctx.OrganizationCode ?? "",
                            UserPhotoUrl = personCreated.Person.PhotoUrl ?? "",
                            LogoUrl = "https://carnetgo.com/logo.png",
                            QrUrl = ""
                        };

                        using var pdfStream = new MemoryStream();
                        await _cardPdfService.GenerateCardAsync(template, userData, pdfStream);
                        pdfStream.Position = 0;

                        var (publicUrl, _) = await _storageService.UploadAsync(
                            pdfStream,
                            "application/pdf",
                            $"Cards/Carnet_{pdpId}.pdf"
                        );

                        await _pdpBusiness.UpdatePdfUrlAsync(pdpId.Value, publicUrl);
                    }
                    catch (Exception pdfEx)
                    {
                        _logger.LogWarning(pdfEx, "Error generando PDF para PDP {PdpId}", pdpId);
                    }

                    await _uow.CommitAsync();

                    // === Resultado ===
                    rowRes.Success = true;
                    rowRes.Created = true;
                    rowRes.Message = "Importación exitosa.";
                    result.SuccessCount++;

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
                        IssuedCardId = pdpId,
                        CardId = cardConfigId,
                        UpdatedPhoto = rowRes.UpdatedPhoto
                    });

                    result.Rows.Add(rowRes);
                }
            }

            // === Completar ===
            await _history.CompleteBatchAsync(new ImportBatchCompleteDto
            {
                ImportBatchId = batchId,
                SuccessCount = result.SuccessCount,
                ErrorCount = result.ErrorCount
            });

            result.TableRows = tableRows;

            await _notificationsBusiness.SendTemplateAsync(
                NotificationTemplateType.BulkImportSuccess,
                parsed.Count,
                fileName
            );

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

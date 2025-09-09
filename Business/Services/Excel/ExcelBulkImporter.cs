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

        public async Task<BulkImportResultDto> ImportAsync(Stream excelStream, ImportContextCard ctx)
        {
            var parsed = await _parser.ParseAsync(excelStream);
            var result = new BulkImportResultDto { TotalRows = parsed.Count };

            // 🧾 Metadatos desde la operación:
            var fileName = _excel.GetFileName(excelStream);       // nombre del archivo, si se puede
            var startedBy = _currentUser.UserName ?? _currentUser.UserId; // del token

            // Snapshot de contexto (negocio)
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

            // Lote
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
                    try
                    {
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
                    }
                    catch (ValidationException vex) // <-- conserva texto específico
                    {
                        _logger.LogWarning(vex, "Validación creando persona en fila {Row}", row.RowNumber);
                        throw new InvalidOperationException($"No se pudo registrar la persona: {vex.Message}", vex);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error creando persona en fila {Row}", row.RowNumber);
                        throw new InvalidOperationException($"No se pudo registrar la persona. {Innermost(ex)}", ex);
                    }



                    // === STEP 2: Vincular PDP ===
                    try
                    {
                        var pdpSaved = await _pdpBusiness.Save(new PersonDivisionProfileDtoRequest
                        {
                            PersonId = personId.Value,
                            InternalDivisionId = ctx.InternalDivisionId,
                            ProfileId = ctx.ProfileId,
                            isCurrentlySelected = true,
                        });
                        pdpId = pdpSaved.Id;
                    }
                    catch (ValidationException vex)
                    {
                        _logger.LogWarning(vex, "Validación vinculando PDP en fila {Row}", row.RowNumber);
                        throw new InvalidOperationException($"No se pudo asignar el perfil/división: {vex.Message}", vex);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error vinculando PDP en fila {Row}", row.RowNumber);
                        throw new InvalidOperationException($"No se pudo asignar el perfil/división. {Innermost(ex)}", ex);
                    }



                    // === STEP 3: Subir/actualizar foto (si viene) ===
                    if (row.PhotoBytes is { Length: > 0 })
                    {
                        try
                        {
                            var (ext, contentType) = ImageFormatValidator.EnsureSupported(row.PhotoBytes, row.PhotoExtension);
                            using var ms = new MemoryStream(row.PhotoBytes);
                            await _personBusiness.UpsertPersonPhotoAsync(personId.Value, ms, contentType, $"excel-upload{ext}");
                            rowRes.UpdatedPhoto = true;
                        }
                        catch (ValidationException vex)
                        {
                            _logger.LogWarning(vex, "Validación subiendo foto en fila {Row}", row.RowNumber);
                            throw new InvalidOperationException($"La foto no se pudo guardar: {vex.Message}", vex);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Error subiendo foto en fila {Row}", row.RowNumber);
                            throw new InvalidOperationException($"La foto no se pudo guardar. {Innermost(ex)}", ex);
                        }
                    }



                    // === STEP 4: Crear carnet ===
                    try
                    {
                        var card = await _cardBusiness.Save(new CardDtoRequest
                        {
                            CreationDate = ctx.ValidFrom,
                            ExpirationDate = ctx.ValidTo,
                            PersonDivissionProfileId = pdpId.Value,
                            StatusId = 1
                        });
                        cardId = card.Id;
                    }
                    catch (ValidationException vex)
                    {
                        _logger.LogWarning(vex, "Validación creando carnet en fila {Row}", row.RowNumber);
                        throw new InvalidOperationException($"No se pudo generar el carnet: {vex.Message}", vex);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error creando carnet en fila {Row}", row.RowNumber);
                        throw new InvalidOperationException($"No se pudo generar el carnet. {Innermost(ex)}", ex);
                    }

                    await _uow.CommitAsync();

                    rowRes.Success = true;
                    rowRes.Created = true;
                    rowRes.EmailSent = personCreated.EmailSent;
                    rowRes.Message = "Importación exitosa.";
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    await _uow.RollbackAsync();
                    rowRes.Success = false;
                    rowRes.Created = false;
                    rowRes.Message = ex is InvalidOperationException ioe ? ioe.Message : Innermost(ex);
                    result.ErrorCount++;
                    _logger.LogWarning(ex, "Error importando fila {Row}", row.RowNumber);
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

            return result;
        }

        // helper muy pequeño
        private static string Innermost(Exception ex)
        {
            while (ex.InnerException != null) ex = ex.InnerException;
            return ex.Message;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Organizational.Assignment;
using Business.Interfaces.Security;
using Business.Interfases.Storage;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Organizational.Assigment.Request;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.DTOs.Specifics;
using Microsoft.Extensions.Logging;

namespace Business.Services.Excel
{
    public class ExcelBulkImporter : IExcelBulkImporter
    {
        private readonly ILogger _logger;
        private readonly IExcelPersonParser _parser;
        private readonly IPersonBusiness _personBusiness;
        private readonly IPersonDivisionProfileBusiness _pdpBusiness;
        private readonly ICardBusiness _cardBusiness;
        private readonly IFileStorageService _storage;

        public ExcelBulkImporter(
            ILogger<ExcelBulkImporter> logger,
            IExcelPersonParser parser,
            IPersonBusiness personBusiness,
            IPersonDivisionProfileBusiness pdpBusiness,
            ICardBusiness cardBusiness,
            IFileStorageService storage)
        {
            _logger = logger;
            _parser = parser;
            _personBusiness = personBusiness;
            _pdpBusiness = pdpBusiness;
            _cardBusiness = cardBusiness;
            _storage = storage;
        }

        public async Task<BulkImportResultDto> ImportAsync(Stream excelStream, ImportContextCard ctx)
        {
            var parsed = await _parser.ParseAsync(excelStream);
            var result = new BulkImportResultDto
            {
                TotalRows = parsed.Count
            };

            foreach (var row in parsed)
            {
                var rowRes = new BulkRowResult { RowNumber = row.RowNumber };
                try
                {
                    // 1) Crea Persona + Usuario
                    var registrer = new PersonRegistrer
                    {
                        Person = row.Person,
                        User = new UserDtoRequest
                        {
                            UserName = row.Person.Email,       // típico
                            Password = row.TempPassword ?? "ChangeMe.123" // fallback
                        }
                    };

                    (PersonRegistrerDto, bool?) personCreated = await _personBusiness.SavePersonAndUser(registrer);
                    int personId = personCreated.Item1.Person.Id;

                    // 2) Subir Foto (si hay)
                    string? publicUrl = null;
                    string? storagePath = null;
                    if (row.PhotoBytes != null && row.PhotoBytes.Length > 0)
                    {
                        var ext = string.IsNullOrWhiteSpace(row.PhotoExtension) ? ".jpg" : row.PhotoExtension!;
                        var destPath = $"people/{ctx.OrganizationCode}/{ctx.OrganizationalUnitCode}/{ctx.InternalDivisionCode}/{personId}/{Guid.NewGuid()}{ext}";

                        using var ms = new MemoryStream(row.PhotoBytes);
                        var contentType = GetContentTypeByExt(ext); // "image/jpeg" / "image/png"...
                        var upload = await _storage.UploadAsync(ms, contentType, destPath);

                        publicUrl = upload.PublicUrl;
                        storagePath = upload.StoragePath;
                    }

                    PersonDivisionProfileDtoRequest pdp = new PersonDivisionProfileDtoRequest
                    {
                        PersonId = personId,
                        InternalDivisionId = ctx.InternalDivisionId,
                        ProfileId = ctx.ProfileId,
                        isCurrentlySelected = true,
                    };

                    // 3) Vincular PersonDivisionProfile
                    PersonDivisionProfileDto pdpSave = await _pdpBusiness.Save(pdp);

                    // 4) Crear Card (con vigencia y foto)
                    await _cardBusiness.Save(new CardDtoRequest
                    {
                        CreationDate = ctx.ValidFrom,
                        ExpirationDate = ctx.ValidTo,
                        PersonDivissionProfileId = pdpSave.Id,
                        StatusId = 1
                    });

                    rowRes.Success = true;
                    rowRes.Created = true;
                    rowRes.EmailSent = personCreated.Item1.EmailSent;
                    rowRes.Message = "OK";
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    rowRes.Success = false;
                    rowRes.Message = ShortError(ex);
                    result.ErrorCount++;
                    _logger.LogWarning(ex, "Error importando fila {Row}", row.RowNumber);
                }

                result.Rows.Add(rowRes);
            }

            return result;
        }

        private static string GetContentTypeByExt(string ext)
        {
            ext = ext.ToLowerInvariant();
            return ext switch
            {
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "image/jpeg"
            };
        }

        private static string ShortError(Exception ex) =>
            ex is InvalidOperationException ioe ? ioe.Message : ex.Message;
    }
}

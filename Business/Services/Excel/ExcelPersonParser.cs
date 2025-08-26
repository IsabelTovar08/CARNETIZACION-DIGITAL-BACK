using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Business.Classes;
using Business.Interfaces.Security;
using ClosedXML.Excel;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Microsoft.Extensions.Logging;

namespace Business.Services.Excel
{
    /// <summary>
    /// Encargado SOLO de leer el Excel, validar y mapear a PersonRegistrer.
    /// </summary>
    public class ExcelPersonParser : IExcelPersonParser
    {
        public readonly ILogger _logger;
        public readonly IPersonBusiness _personBusiness;

        public ExcelPersonParser(ILogger<ExcelPersonParser> logger, IPersonBusiness personBusiness)
        {
            _logger = logger;
            _personBusiness = personBusiness;
        }

        public async Task<BulkImportResultDto> ImportFromExcelAsync(Stream excelStream)
        {
            // Español: Validación básica del stream
            if (excelStream == null || !excelStream.CanRead)
                throw new ArgumentException("El stream de Excel es nulo o no es legible.");

            var result = new BulkImportResultDto();

            using var workbook = new XLWorkbook(excelStream);
            var ws = workbook.Worksheets.FirstOrDefault();
            if (ws == null) throw new ArgumentException("No se encontró una hoja válida en el Excel.");

            // Español: Suponemos fila 1 = encabezados, fila 2..N = datos
            const int HEADER_ROW = 1;
            const int FIRST_DATA_ROW = 2;

            // Español: Mapea columnas por índice (A=1, B=2, ... K=11)
            const int COL_FIRSTNAME = 1;  // A
            const int COL_MIDDLENAME = 2;  // B
            const int COL_LASTNAME = 3;  // C
            const int COL_SECONDLASTNAME = 4;  // D
            const int COL_DOCUMENTTYPEID = 5;  // E
            const int COL_DOCUMENTNUMBER = 6;  // F
            const int COL_BLOODTYPEID = 7;  // G
            const int COL_PHONE = 8;  // H
            const int COL_EMAIL = 9;  // I
            const int COL_ADDRESS = 10; // J
            const int COL_CITYID = 11; // K

            var lastRow = ws.LastRowUsed().RowNumber();
            if (lastRow < FIRST_DATA_ROW)
            {
                result.TotalRows = 0;
                return result;
            }

            result.TotalRows = lastRow - HEADER_ROW;

            // Español: Evita duplicados dentro del mismo archivo (rendimiento O(1))
            var emailsInFile = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var docsInFile = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int row = FIRST_DATA_ROW; row <= lastRow; row++)
            {
                var rowRes = new BulkRowResult { RowNumber = row };
                try
                {
                    var dto = new PersonDtoRequest
                    {
                        FirstName = ws.Cell(row, COL_FIRSTNAME).GetString().Trim(),
                        MiddleName = StringHelpers.TrimToNull(ws.Cell(row, COL_MIDDLENAME).GetString()),
                        LastName = ws.Cell(row, COL_LASTNAME).GetString().Trim(),
                        SecondLastName = StringHelpers.TrimToNull(ws.Cell(row, COL_SECONDLASTNAME).GetString()),
                        DocumentTypeId = ExcelHelpers.TryGetIntNull(ws.Cell(row, COL_DOCUMENTTYPEID)),
                        DocumentNumber = StringHelpers.TrimToNull(ws.Cell(row, COL_DOCUMENTNUMBER).GetString()),
                        BloodTypeId = ExcelHelpers.TryGetIntNull(ws.Cell(row, COL_BLOODTYPEID)),
                        Phone = StringHelpers.TrimToNull(ws.Cell(row, COL_PHONE).GetString()),
                        Email = ws.Cell(row, COL_EMAIL).GetString().Trim(),
                        Address = ws.Cell(row, COL_ADDRESS).GetString().Trim(),
                        CityId = ws.Cell(row, COL_CITYID).GetValue<int>()
                    };

                    // ===== Validaciones baratas (sin DB) =====
                    var validationError = ValidateRow(dto, emailsInFile, docsInFile);
                    if (validationError != null)
                        throw new InvalidOperationException(validationError);

                    // ===== Generar password temporal =====
                    var tempPassword = GenerateTempPassword();

                    // ===== Construcción para tu flujo existente =====
                    var personUser = new PersonRegistrer
                    {
                        Person = new PersonDtoRequest
                        {
                            FirstName = dto.FirstName,
                            MiddleName = dto.MiddleName,
                            LastName = dto.LastName,
                            SecondLastName = dto.SecondLastName,
                            DocumentTypeId = dto.DocumentTypeId,
                            DocumentNumber = dto.DocumentNumber,
                            BloodTypeId = dto.BloodTypeId,
                            Phone = dto.Phone,
                            Email = dto.Email,
                            Address = dto.Address,
                            CityId = dto.CityId
                        },
                        User = new UserDtoRequest
                        {
                            UserName = dto.Email,       // Español: usualmente user = email
                            Password = tempPassword
                        }
                    };

                    var (saved, emailSent) = await _personBusiness.SavePersonAndUser(personUser);

                    rowRes.Success = true;
                    rowRes.Created = true;
                    rowRes.EmailSent = (bool)(emailSent ?? null);
                    rowRes.Message = "OK";
                    result.SuccessCount++;

                }
                catch (Exception ex)
                {
                    rowRes.Success = false;
                    rowRes.Message = ShortError(ex); // Español: mensaje corto e inteligible
                    result.ErrorCount++;
                    _logger.LogWarning(ex, "Error importando fila {Row}", row);
                }

                result.Rows.Add(rowRes);
            }

            return result;
        }

        // ================== Helpers ==================

        // Español: Validación mínima + duplicados en el archivo
        private static string? ValidateRow(PersonDtoRequest r, HashSet<string> emailsInFile, HashSet<string> docsInFile)
        {
            if (string.IsNullOrWhiteSpace(r.FirstName)) return "FirstName es requerido.";
            if (string.IsNullOrWhiteSpace(r.LastName)) return "LastName es requerido.";
            if (string.IsNullOrWhiteSpace(r.Email)) return "Email es requerido.";
            if (!IsValidEmail(r.Email)) return "Email con formato inválido.";
            if (string.IsNullOrWhiteSpace(r.Address)) return "Address es requerido.";
            if (r.CityId <= 0) return "CityId es inválido o vacío.";

            // Duplicado en el mismo archivo
            if (!emailsInFile.Add(r.Email)) return "Email duplicado en el archivo.";
            if (!string.IsNullOrWhiteSpace(r.DocumentNumber))
            {
                if (!docsInFile.Add(r.DocumentNumber)) return "DocumentNumber duplicado en el archivo.";
            }

            return null;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            }
            catch { return false; }
        }

        private static string GenerateTempPassword(int length = 10)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789@$!#%*?";
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                sb.Append(chars[bytes[i] % chars.Length]);
            return sb.ToString();
        }

        private static string ShortError(Exception ex) =>
            ex is InvalidOperationException ioe ? ioe.Message : ex.Message;



        // ================== Small helpers ==================
        public static class StringHelpers
        {
            // Español: Trim que retorna null si queda vacío
            public static string? TrimToNull(string s)
                => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
        }

        public static class ExcelHelpers
        {
            // Español: Int? a partir de una celda; soporta números o texto numérico
            public static int? TryGetIntNull(IXLCell cell)
            {
                if (cell.IsEmpty()) return null;
                if (cell.TryGetValue<int>(out var v)) return v;
                var s = cell.GetString().Trim();
                return int.TryParse(s, out v) ? v : (int?)null;
            }

        }
    }
}

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
using ClosedXML.Excel.Drawings;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Specifics;
using Microsoft.Extensions.Logging;

namespace Business.Services.Excel
{
    /// <summary>
    /// Encargado SOLO de leer el Excel, validar y mapear a PersonRegistrer.
    /// </summary>
    public class ExcelPersonParser : IExcelPersonParser
    {
        private readonly ILogger _logger;

        public ExcelPersonParser(ILogger<ExcelPersonParser> logger)
        {
            _logger = logger;
        }

        public async Task<IReadOnlyList<ParsedPersonRow>> ParseAsync(Stream excelStream)
        {
            if (excelStream == null || !excelStream.CanRead)
                throw new ArgumentException("El stream de Excel es nulo o no es legible.");

            using var workbook = new XLWorkbook(excelStream);
            var ws = workbook.Worksheets.FirstOrDefault() ?? throw new ArgumentException("No se encontró una hoja válida en el Excel.");

            const int HEADER_ROW = 1;
            const int FIRST_DATA_ROW = 2;

            const int COL_FIRSTNAME = 1;   // A
            const int COL_MIDDLENAME = 2;  // B
            const int COL_LASTNAME = 3;    // C
            const int COL_SECONDLASTNAME = 4;  // D
            const int COL_DOCUMENTTYPEID = 5;  // E
            const int COL_DOCUMENTNUMBER = 6;  // F
            const int COL_BLOODTYPEID = 7;     // G
            const int COL_PHONE = 8;           // H
            const int COL_EMAIL = 9;           // I
            const int COL_ADDRESS = 10;        // J
            const int COL_CITYID = 11;         // K
            const int COL_PHOTO = 12;          // L (📸 foto embebida intersectando esta celda)

            var lastRow = ws.LastRowUsed()?.RowNumber() ?? HEADER_ROW;
            if (lastRow < FIRST_DATA_ROW) return Array.Empty<ParsedPersonRow>();

            // Duplicados en archivo (O(1))
            var emailsInFile = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var docsInFile = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var list = new List<ParsedPersonRow>(lastRow - HEADER_ROW);

            for (int row = FIRST_DATA_ROW; row <= lastRow; row++)
            {
                try
                {
                    var dto = new PersonDtoRequest
                    {
                        FirstName = ws.Cell(row, COL_FIRSTNAME).GetString().Trim(),
                        MiddleName = TrimToNull(ws.Cell(row, COL_MIDDLENAME).GetString()),
                        LastName = ws.Cell(row, COL_LASTNAME).GetString().Trim(),
                        SecondLastName = TrimToNull(ws.Cell(row, COL_SECONDLASTNAME).GetString()),
                        DocumentTypeId = TryGetIntNull(ws.Cell(row, COL_DOCUMENTTYPEID)),
                        DocumentNumber = TrimToNull(ws.Cell(row, COL_DOCUMENTNUMBER).GetString()),
                        BloodTypeId = TryGetIntNull(ws.Cell(row, COL_BLOODTYPEID)),
                        Phone = TrimToNull(ws.Cell(row, COL_PHONE).GetString()),
                        Email = ws.Cell(row, COL_EMAIL).GetString().Trim(),
                        Address = ws.Cell(row, COL_ADDRESS).GetString().Trim(),
                        CityId = ws.Cell(row, COL_CITYID).GetValue<int>()
                    };

                    // ✅ Validaciones "baratas"
                    var validationError = ValidateRow(dto, emailsInFile, docsInFile);
                    if (validationError != null)
                        throw new InvalidOperationException(validationError);

                    // 🔐 Password temporal
                    var tempPassword = GenerateTempPassword();

                    // 📸 Foto: busca la imagen que intersecte la celda [row, COL_PHOTO]
                    byte[]? photoBytes = null;
                    string? photoExt = null;
                    var photoCell = ws.Cell(row, COL_PHOTO).Address;
                    var picture = FindPictureIntersectingCell(ws, photoCell.RowNumber, photoCell.ColumnNumber);

                    if (picture != null)
                    {
                        using var ms = new MemoryStream();
                        picture.ImageStream.Position = 0;   // aseguramos inicio
                        await picture.ImageStream.CopyToAsync(ms);
                        photoBytes = ms.ToArray();

                        // ext tentativa (ClosedXML mapea tipo)
                        photoExt = picture.Format switch
                        {
                            XLPictureFormat.Png => ".png",
                            XLPictureFormat.Jpeg => ".jpg",
                            XLPictureFormat.Gif => ".gif",
                            XLPictureFormat.Bmp => ".bmp",
                            _ => ".jpg"
                        };
                    }

                    list.Add(new ParsedPersonRow
                    {
                        RowNumber = row,
                        Person = dto,
                        TempPassword = tempPassword,
                        PhotoBytes = photoBytes,
                        PhotoExtension = photoExt
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error parseando fila {Row}", row);
                    // Continúa; si quieres, también podrías agregar una marca de error en una colección paralela
                }
            }

            return list;
        }

        // ============ Helpers (parser) ============

        private static IXLPicture? FindPictureIntersectingCell(IXLWorksheet ws, int row, int col)
        {
            var cellRange = ws.Cell(row, col).AsRange(); // 1x1
                                                         // Intersección geométrica simple (ClosedXML expone TopLeftCell/BottomRightCell aprox)
            return ws.Pictures.FirstOrDefault(p =>
            {
                var tl = p.TopLeftCell.Address;
                var br = p.BottomRightCell.Address;
                return row >= tl.RowNumber && row <= br.RowNumber &&
                       col >= tl.ColumnNumber && col <= br.ColumnNumber;
            });
        }

        private static string? ValidateRow(PersonDtoRequest r, HashSet<string> emailsInFile, HashSet<string> docsInFile)
        {
            if (string.IsNullOrWhiteSpace(r.FirstName)) return "FirstName es requerido.";
            if (string.IsNullOrWhiteSpace(r.LastName)) return "LastName es requerido.";
            if (string.IsNullOrWhiteSpace(r.Email)) return "Email es requerido.";
            if (!Regex.IsMatch(r.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) return "Email con formato inválido.";
            if (r.CityId <= 0) return "CityId es inválido o vacío.";

            if (!emailsInFile.Add(r.Email)) return "Email duplicado en el archivo.";
            if (!string.IsNullOrWhiteSpace(r.DocumentNumber))
                if (!docsInFile.Add(r.DocumentNumber)) return "DocumentNumber duplicado en el archivo.";

            return null;
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

        private static string? TrimToNull(string s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

        private static int? TryGetIntNull(IXLCell cell)
        {
            if (cell.IsEmpty()) return null;
            if (cell.TryGetValue<int>(out var v)) return v;
            var s = cell.GetString().Trim();
            return int.TryParse(s, out v) ? v : (int?)null;
        }
    }
}

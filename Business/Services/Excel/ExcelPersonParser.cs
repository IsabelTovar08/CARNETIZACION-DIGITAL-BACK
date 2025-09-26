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
using Utilities.Helpers.Excel;

namespace Business.Services.Excel
{
    public class ExcelPersonParser : IExcelPersonParser
    {
        private readonly ILogger _logger;
        private readonly IExcelReaderHelper _excel;

        public ExcelPersonParser(ILogger<ExcelPersonParser> logger, IExcelReaderHelper excelReaderHelper)
        {
            _logger = logger;
            _excel = excelReaderHelper;
        }

        public async Task<IReadOnlyList<ParsedPersonRow>> ParseAsync(Stream excelStream)
        {
            var ws = _excel.OpenFirstWorksheet(excelStream);

            const int HEADER_ROW = 1;
            const int FIRST_DATA_ROW = 2;

            const int COL_FIRSTNAME = 1;     // A
            const int COL_MIDDLENAME = 2;    // B
            const int COL_LASTNAME = 3;      // C
            const int COL_SECONDLASTNAME = 4;// D
            const int COL_DOCUMENTTYPEID = 5;// E
            const int COL_DOCUMENTNUMBER = 6;// F
            const int COL_BLOODTYPEID = 7;   // G
            const int COL_PHONE = 8;         // H
            const int COL_EMAIL = 9;         // I
            const int COL_ADDRESS = 10;      // J
            const int COL_CITYID = 11;       // K
            const int COL_PHOTO = 12;        // L

            var lastRow = _excel.GetLastUsedRow(ws);
            if (lastRow < FIRST_DATA_ROW) return Array.Empty<ParsedPersonRow>();

            var emailsInFile = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var docsInFile = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var list = new List<ParsedPersonRow>(lastRow - HEADER_ROW);

            for (int row = FIRST_DATA_ROW; row <= lastRow; row++)
            {
                try
                {
                    var dto = new PersonDtoRequest
                    {
                        FirstName = _excel.ReadString(ws, row, COL_FIRSTNAME),
                        MiddleName = _excel.ReadNullableString(ws, row, COL_MIDDLENAME),
                        LastName = _excel.ReadString(ws, row, COL_LASTNAME),
                        SecondLastName = _excel.ReadNullableString(ws, row, COL_SECONDLASTNAME),
                        DocumentTypeId = _excel.ReadNullableInt(ws, row, COL_DOCUMENTTYPEID),
                        DocumentNumber = _excel.ReadNullableString(ws, row, COL_DOCUMENTNUMBER),
                        BloodTypeId = _excel.ReadNullableInt(ws, row, COL_BLOODTYPEID),
                        Phone = _excel.ReadNullableString(ws, row, COL_PHONE),
                        Email = _excel.ReadString(ws, row, COL_EMAIL),
                        Address = _excel.ReadString(ws, row, COL_ADDRESS),
                        CityId = _excel.ReadIntOrDefault(ws, row, COL_CITYID, 0)
                    };

                    // Validaciones rápidas y duplicados en el archivo
                    var validationError = ValidateRow(dto, emailsInFile, docsInFile);
                    if (validationError != null)
                        throw new InvalidOperationException(validationError);

                    var tempPassword = GenerateTempPassword();

                    // Foto embebida en la celda L(row); si existe, trae bytes y extensión
                    var (photoBytes, photoExt) = _excel.ReadPictureAtCell(ws, row, COL_PHOTO);

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
                    // Seguimos con las demás filas
                }
            }

            return list;
        }

        // ============ Validaciones & utilidades  ============

        private static string? ValidateRow(
            PersonDtoRequest r,
            HashSet<string> emailsInFile,
            HashSet<string> docsInFile)
        {
            if (string.IsNullOrWhiteSpace(r.FirstName)) return "El nombre es obligatorio.";
            if (string.IsNullOrWhiteSpace(r.LastName)) return "El apellido es obligatorio.";
            if (string.IsNullOrWhiteSpace(r.Email)) return "El correo electrónico es obligatorio.";
            if (!Regex.IsMatch(r.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) return "Formato de correo inválido.";
            if (r.CityId <= 0) return "El identificador de ciudad es inválido o está vacío.";

            if (!emailsInFile.Add(r.Email)) return "Correo electrónico duplicado en el archivo.";
            if (!string.IsNullOrWhiteSpace(r.DocumentNumber))
                if (!docsInFile.Add(r.DocumentNumber!)) return "Número de documento duplicado en el archivo.";

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
    }
}

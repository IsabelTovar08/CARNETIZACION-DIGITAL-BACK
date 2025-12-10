using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Business.Interfaces.Security;
using ClosedXML.Excel;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.Specifics;
using Microsoft.Extensions.Logging;
using Utilities.Helpers.Excel;
using static Utilities.Helpers.GeneratePassword;

namespace Business.Services.Excel
{
    /// <summary>
    /// Servicio que lee un archivo Excel de personas y crea una lista de filas parseadas.
    /// Optimizado para omitir filas y columnas vacías innecesarias.
    /// </summary>
    public class ExcelPersonParser : IExcelPersonParser
    {
        private readonly ILogger _logger;
        private readonly IExcelReaderHelper _excel;

        public ExcelPersonParser(ILogger<ExcelPersonParser> logger, IExcelReaderHelper excelReaderHelper)
        {
            _logger = logger;
            _excel = excelReaderHelper;
        }

        /// <summary>
        /// Parsea un archivo Excel de personas, validando y evitando leer columnas vacías.
        /// </summary>
        /// <summary>
        /// Parsea un archivo Excel de personas leyendo TipoDoc, TipoSangre y Ciudad
        /// desde las 3 últimas columnas del archivo.
        /// </summary>
        public async Task<IReadOnlyList<ParsedPersonRow>> ParseAsync(Stream excelStream)
        {
            var ws = _excel.OpenFirstWorksheet(excelStream);

            const int HEADER_ROW = 1;
            const int FIRST_DATA_ROW = 2;

            // ============================
            // 🔥 Determinar columnas dinámicas
            // ============================
            int lastColumn = ws.LastColumnUsed().ColumnNumber();

            int COL_DOCUMENTTYPEID = lastColumn - 1; // Tipo Doc
            int COL_BLOODTYPEID = lastColumn - 2;    // Tipo Sangre
            int COL_CITYID = lastColumn;             // Ciudad

            int COL_PHOTO = 12;

            // Columnas fijas del inicio
            const int COL_FIRSTNAME = 1;
            const int COL_MIDDLENAME = 2;
            const int COL_LASTNAME = 3;
            const int COL_SECONDLASTNAME = 4;
            const int COL_DOCUMENTNUMBER = 6;
            const int COL_PHONE = 9;
            const int COL_EMAIL = 8;
            const int COL_ADDRESS = 10;

            var lastRow = _excel.GetLastUsedRow(ws);
            if (lastRow < FIRST_DATA_ROW) return Array.Empty<ParsedPersonRow>();

            var emailsInFile = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var docsInFile = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var list = new List<ParsedPersonRow>(lastRow - HEADER_ROW);

            for (int row = FIRST_DATA_ROW; row <= lastRow; row++)
            {
                try
                {
                    if (RowIsEmpty(ws, row, COL_FIRSTNAME, COL_LASTNAME, COL_EMAIL))
                        continue;

                    var dto = new PersonDtoRequest
                    {
                        FirstName = _excel.ReadString(ws, row, COL_FIRSTNAME),
                        MiddleName = _excel.ReadNullableString(ws, row, COL_MIDDLENAME),
                        LastName = _excel.ReadString(ws, row, COL_LASTNAME),
                        SecondLastName = _excel.ReadNullableString(ws, row, COL_SECONDLASTNAME),

                        // ================================
                        // 🔥 Campos movidos a las columnas finales
                        // ================================
                        DocumentType = (Utilities.Enums.Specifics.DocumentType?)_excel.ReadNullableInt(ws, row, COL_DOCUMENTTYPEID),
                        BloodType = (Utilities.Enums.Specifics.BloodType?)_excel.ReadNullableInt(ws, row, COL_BLOODTYPEID),
                        CityId = _excel.ReadIntOrDefault(ws, row, COL_CITYID, 0),

                        DocumentNumber = _excel.ReadNullableString(ws, row, COL_DOCUMENTNUMBER),
                        Phone = _excel.ReadNullableString(ws, row, COL_PHONE),
                        Email = _excel.ReadString(ws, row, COL_EMAIL),
                        Address = _excel.ReadNullableString(ws, row, COL_ADDRESS)
                    };

                    var validationError = ValidateRow(dto, emailsInFile, docsInFile);
                    if (validationError != null)
                        throw new InvalidOperationException(validationError);

                    var tempPassword = GenerateTempPassword();

                    var (photoBytes, photoExt) = _excel.HasPictureAtCell(ws, row, COL_PHOTO)
                        ? _excel.ReadPictureAtCell(ws, row, COL_PHOTO)
                        : (null, null);

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
                }
            }

            if (list.Count == 0)
                throw new InvalidOperationException("El archivo Excel no contiene información válida para procesar.");


            return list;
        }

        /// <summary>
        /// Verifica si una fila está vacía o no tiene los datos esenciales.
        /// </summary>
        private static bool RowIsEmpty(IXLWorksheet ws, int row, params int[] requiredCols)
        {
            foreach (var col in requiredCols)
            {
                var cellValue = ws.Cell(row, col).GetValue<string>()?.Trim();
                if (!string.IsNullOrWhiteSpace(cellValue))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Validaciones de campos requeridos y duplicados en el archivo.
        /// </summary>
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
    }
}

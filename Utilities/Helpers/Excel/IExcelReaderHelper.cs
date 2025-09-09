using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace Utilities.Helpers.Excel
{
    /// <summary>
    /// Contrato para operaciones de lectura de Excel.
    /// Abstrae ClosedXML para permitir cambiar de librería sin tocar negocio.
    /// </summary>
    public interface IExcelReaderHelper
    {
        IXLWorksheet OpenFirstWorksheet(Stream excelStream);
        int GetLastUsedRow(IXLWorksheet ws);

        string ReadString(IXLWorksheet ws, int row, int col, bool trim = true);
        string? ReadNullableString(IXLWorksheet ws, int row, int col, bool trim = true);

        int? ReadNullableInt(IXLWorksheet ws, int row, int col);
        int ReadIntOrDefault(IXLWorksheet ws, int row, int col, int defaultValue = 0);

        /// <summary>
        /// Obtiene bytes de la imagen que intersecte la celda (row,col), y su extensión sugerida.
        /// </summary>
        (byte[]? bytes, string? extension) ReadPictureAtCell(IXLWorksheet ws, int row, int col);

        string GetFileName(Stream stream);
    }
}

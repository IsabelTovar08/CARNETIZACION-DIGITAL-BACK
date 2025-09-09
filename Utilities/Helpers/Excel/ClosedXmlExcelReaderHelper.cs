using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;

namespace Utilities.Helpers.Excel
{
    public class ClosedXmlExcelReaderHelper : IExcelReaderHelper
    {
        public IXLWorksheet OpenFirstWorksheet(Stream excelStream)
        {
            if (excelStream == null || !excelStream.CanRead)
                throw new ArgumentException("El archivo de Excel es nulo o no se puede leer.");

            var wb = new XLWorkbook(excelStream);
            var ws = wb.Worksheets.FirstOrDefault()
                     ?? throw new ArgumentException("No se encontró una hoja válida en el archivo de Excel.");
            return ws;
        }

        public int GetLastUsedRow(IXLWorksheet ws)
        {
            return ws.LastRowUsed()?.RowNumber() ?? 0;
        }

        public string ReadString(IXLWorksheet ws, int row, int col, bool trim = true)
        {
            var s = ws.Cell(row, col).GetString();
            return trim ? s.Trim() : s;
        }

        public string? ReadNullableString(IXLWorksheet ws, int row, int col, bool trim = true)
        {
            var s = ws.Cell(row, col).GetString();
            if (string.IsNullOrWhiteSpace(s)) return null;
            return trim ? s.Trim() : s;
        }

        public int? ReadNullableInt(IXLWorksheet ws, int row, int col)
        {
            var cell = ws.Cell(row, col);
            if (cell.IsEmpty()) return null;
            if (cell.TryGetValue<int>(out var v)) return v;

            var s = cell.GetString().Trim();
            return int.TryParse(s, out v) ? v : (int?)null;
        }

        public int ReadIntOrDefault(IXLWorksheet ws, int row, int col, int defaultValue = 0)
        {
            var cell = ws.Cell(row, col);
            if (cell.TryGetValue<int>(out var v)) return v;

            var s = cell.GetString().Trim();
            return int.TryParse(s, out v) ? v : defaultValue;
        }

        public (byte[]? bytes, string? extension) ReadPictureAtCell(IXLWorksheet ws, int row, int col)
        {
            var pic = FindPictureIntersectingCell(ws, row, col);
            if (pic == null) return (null, null);

            using var ms = new MemoryStream();
            pic.ImageStream.Position = 0;
            pic.ImageStream.CopyTo(ms);
            var bytes = ms.ToArray();

            var ext = pic.Format switch
            {
                XLPictureFormat.Png => ".png",
                XLPictureFormat.Jpeg => ".jpg",
                XLPictureFormat.Gif => ".gif",
                XLPictureFormat.Bmp => ".bmp",
                _ => ".jpg"
            };

            return (bytes, ext);
        }

        public string GetFileName(Stream stream)
        {
            // Si es FileStream, usamos su Name. Si no, fallback.
            if (stream is FileStream fs && !string.IsNullOrWhiteSpace(fs.Name))
                return Path.GetFileName(fs.Name);

            // (Opcional) intenta leer desde Position=0 firmas OOXML para .xlsx y no el nombre; el nombre NO viaja en el stream.
            // Así que si no es FileStream, no hay forma confiable de saberlo. Usamos un nombre por defecto:
            return "upload.xlsx";
        }

        // ---------- Privado ----------
        private static IXLPicture? FindPictureIntersectingCell(IXLWorksheet ws, int row, int col)
        {
            return ws.Pictures.FirstOrDefault(p =>
            {
                var tl = p.TopLeftCell.Address;
                var br = p.BottomRightCell.Address;
                return row >= tl.RowNumber && row <= br.RowNumber &&
                       col >= tl.ColumnNumber && col <= br.ColumnNumber;
            });
        }
    }
}

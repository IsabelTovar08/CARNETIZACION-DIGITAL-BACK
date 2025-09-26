using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace Business.Services.Excel
{
    public class ExcelReaderService
    {
        public List<List<string>> ReadPreview(string filePath, int previewRows = 50)
        {
            using var wb = new XLWorkbook(filePath);
            var ws = wb.Worksheets.First();
            var rows = ws.RangeUsed().RowsUsed().ToList();

            var preview = new List<List<string>>();
            foreach (var row in rows.Take(previewRows))
            {
                preview.Add(row.Cells().Select(c => c.GetString()).ToList());
            }
            return preview;
        }
    }
}

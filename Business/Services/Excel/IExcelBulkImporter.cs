using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Specifics;

namespace Business.Services.Excel
{
    public interface IExcelBulkImporter
    {
        Task<BulkImportResultDto> ImportAsync(Stream excelStream, ImportContextCard ctx);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.ModelSecurity.Response;

namespace Business.Services.Excel
{
    /// <summary>
    /// Parseo y validación ligera de Excel a filas listas para procesar.
    /// </summary>
    public interface IExcelPersonParser
    {
        Task<BulkImportResultDto> ImportFromExcelAsync(Stream file);
    }
}

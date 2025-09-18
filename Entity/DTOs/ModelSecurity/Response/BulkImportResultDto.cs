using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Specifics;

namespace Entity.DTOs.ModelSecurity.Response
{
    // Resumen global de la importación
    public class BulkImportResultDto
    {
        public int TotalRows { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public List<BulkRowResult> Rows { get; set; } = new();

        /// <summary>Filas en formato optimizado para tabla de Angular.</summary>
        public List<ImportBatchRowTableDto> TableRows { get; set; } = new();
    }
}

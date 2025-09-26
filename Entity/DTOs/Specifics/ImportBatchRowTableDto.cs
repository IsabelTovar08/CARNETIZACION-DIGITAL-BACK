using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics
{
    /// <summary>
    /// DTO optimizado para la tabla en Angular.
    /// </summary>
    /// <summary>
    /// DTO resumido para mostrar en la tabla de Angular.
    /// </summary>
    public class ImportBatchRowTableDto
    {
        public int RowNumber { get; set; }
        public string? Photo { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Org { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}

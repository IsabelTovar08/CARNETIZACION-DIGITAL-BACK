using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics
{
    // DTO simple para registrar filas en el historial de carga masiva a través de excel
    public class ImportBatchRowLog
    {
        public int RowNumber { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int? PersonId { get; set; }
        public int? PersonDivisionProfileId { get; set; }
        public int? CardId { get; set; }
        public bool UpdatedPhoto { get; set; }
    }
}

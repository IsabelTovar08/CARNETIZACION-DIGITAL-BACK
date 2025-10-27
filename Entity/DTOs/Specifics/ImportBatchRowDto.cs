using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics
{
    public class ImportBatchRowDto
    {
        public int ImportBatchId { get; set; }
        public int RowNumber { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int? PersonId { get; set; }
        public string? PersonName { get; set; }
        public int? PersonDivisionProfileId { get; set; }
        public int? CardId { get; set; }
        public bool UpdatedPhoto { get; set; }
        public int? IssuedCardId { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Base;
using Entity.Models.ModelSecurity;
using Entity.Models.Organizational.Assignment;

namespace Entity.Models.Operational.BulkLoading
{
    public class ImportBatchRow : BaseModel
    {
        public int ImportBatchId { get; set; }
        public ImportBatch Batch { get; set; } = default!;

        public int RowNumber { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }

        public int? PersonId { get; set; }
        public int? PersonDivisionProfileId { get; set; }
        public int? CardId { get; set; }
        public bool UpdatedPhoto { get; set; }

        public PersonDivisionProfile PersonDivisionProfile { get; set; }
        public Card Card { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Base;

namespace Entity.Models.Operational.BulkLoading
{
    public class ImportBatch : BaseModel
    {
        public string Source { get; set; } = "Excel";
        public string? FileName { get; set; }
        public int? StartedBy { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public int TotalRows { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }

        public string? ContextJson { get; set; }

        /// <summary>
        /// Relación con el usuario que inició la importación
        /// </summary>
        public User? StartedByUser { get; set; }
        public ICollection<ImportBatchRow> Rows { get; set; } = new List<ImportBatchRow>();
    }
}

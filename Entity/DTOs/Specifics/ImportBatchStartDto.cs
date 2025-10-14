using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics
{
    public class ImportBatchStartDto
    {
        public string Source { get; set; } = "Excel";
        public string? FileName { get; set; }
        public int? StartedBy { get; set; }
        public int TotalRows { get; set; }
        public string? ContextJson { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics
{
    public class ImportBatchCompleteDto
    {
        public int ImportBatchId { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
    }
}

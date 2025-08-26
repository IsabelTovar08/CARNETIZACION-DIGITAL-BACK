using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class CompanyRequest
    {
        public int CompanyRequestId { get; set; }
        public string RequestedName { get; set; } = null!;
        public string RequestedBy { get; set; } = null!;
        public DateTime RequestedAt { get; set; }
        public bool? Approved { get; set; } 
    }
}

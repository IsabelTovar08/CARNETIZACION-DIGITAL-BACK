using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Operational
{
    public class EventTargetAudienceDto : BaseDTO
    {
        public string Type { get; set; }
        public  int ReferenceId { get; set; }
        public string ReferenceName { get; set; }

        public int EventId { get; set; }
        public string EventName { get; set; }
    }
}

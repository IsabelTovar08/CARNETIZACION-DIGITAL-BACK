using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Operational.Request
{
    public class EventSupervisorDtoRequest : BaseDtoRequest
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
    }
}

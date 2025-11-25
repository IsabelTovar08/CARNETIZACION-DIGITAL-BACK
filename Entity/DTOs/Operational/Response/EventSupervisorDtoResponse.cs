using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Operational.Response
{
    public class EventSupervisorDtoResponse : BaseDTO
    {
        public int EventId { get; set; }
        public string? EventName { get; set; }

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
    }
}

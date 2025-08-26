using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Operational
{
    public  class EventDtoResponse : GenericDto
    {
        public string Code { get; set; }
        public string? Description { get; set; }

        public DateTime? ScheduleDate { get; set; }
        public DateTime? ScheduleTime { get; set; }
        public DateTime? EventStart { get; set; }
        public DateTime? EventEnd { get; set; }

        public int? SheduleId { get; set; }

        public int EventTypeId { get; set; }
        public string EventTypeName { get; set; }

        public bool Ispublic { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }
}

using Entity.Models.Base;
using Entity.Models.Operational;
using Entity.Models.Organizational.Structure;
using Entity.Models.Parameter;
using System;
using System.Collections.Generic;
namespace Entity.Models.Organizational
{
    public class Event : GenericModel
    {
        public string Code { get; set; }
        public string? Description { get; set; }

        public DateTime? ScheduleDate { get; set; }
        public DateTime? ScheduleTime { get; set; }
        public DateTime? EventStart { get; set; }
        public DateTime? EventEnd { get; set; }

        public int? ScheduleId { get; set; }


        public bool IsPublic { get; set; }
        public int StatusId { get; set; }

        public int EventTypeId { get; set; }
        public EventType? EventType { get; set; }

        public string? Days { get; set; }

        public ICollection<EventTargetAudience> EventTargetAudiences { get; set; } = new List<EventTargetAudience>();
        public Status? Status { get; set; }
        public Schedule? Shedule { get; set; }
        public ICollection<EventAccessPoint> EventAccessPoints { get; set; } = new List<EventAccessPoint>();



    }
}

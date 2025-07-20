using System;
using System.Collections.Generic;
using Entity.Models.Base;
using Entity.Models.Others;

namespace Entity.Models.Event
{
    public class Event : GenericModel
    {
        public string Code { get; set; }

        public DateTime ScheduleDate { get; set; }
        public TimeSpan ScheduleTime { get; set; }
        public DateTime EventStart { get; set; }
        public DateTime EventEnd { get; set; }

        public bool IsPublic { get; set; }
        public int StatusId { get; set; }

        public int EventTypeId { get; set; }
        public EventType EventType { get; set; }

        public List<AccessPoint>? AccessPoints { get; set; }

        public List<EventTargetAudience> EventTargetAudiences { get; set; }
        public Status Status { get; set; }

    }
}

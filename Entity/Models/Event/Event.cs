using System;
using System.Collections.Generic;
using Entity.Models.Base;

namespace Entity.Models.Event
{
    public class Event : GenericModel
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public DateTime Date { get; set; }
        public DateTime ScheduleDate { get; set; }
        public TimeSpan ScheduleTime { get; set; }
        public DateTime EventStart { get; set; }
        public DateTime EventEnd { get; set; }

        public bool IsPublic { get; set; }

        public int EventTypeId { get; set; }
        public EventType EventType { get; set; }

        public int AccessPointId { get; set; }
        public AccessPoint AccessPoint { get; set; }

        public ICollection<EventTargetAudience> EventTargetAudiences { get; set; }
    }
}

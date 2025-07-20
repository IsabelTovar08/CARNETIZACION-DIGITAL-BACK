using System.Collections.Generic;
using Entity.Models.Base;
using Entity.Models.Others;

namespace Entity.Models.Event
{
    public class AccessPoint : GenericModel
    {
        public string Type { get; set; }
        public string? Description { get; set; }
        public int EventId { get; set; }

        public Event Event { get; set; }

        public List<Attendance>? AttendancesEntry { get; set; }
        public List<Attendance>? AttendancesExit { get; set; }

        public int TypeId { get; set; }

        public CustomType AccessPointType { get; set; }
    }
}

using System.Collections.Generic;
using Entity.Models.Base;
using Entity.Models.Parameter;

namespace Entity.Models.Organizational
{
    public class AccessPoint : GenericModel
    {
        public string? Description { get; set; }
        public int EventId { get; set; }
        public int TypeId { get; set; }


        public Event Event { get; set; }

        public List<Attendance>? AttendancesEntry { get; set; }
        public List<Attendance>? AttendancesExit { get; set; }


        public CustomType AccessPointType { get; set; }
    }
}

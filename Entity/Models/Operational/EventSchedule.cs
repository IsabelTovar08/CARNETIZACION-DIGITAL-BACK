using Entity.DTOs.Base;
using Entity.Models.Base;
using Entity.Models.Organizational;
using Entity.Models.Organizational.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.Operational
{
    public class EventSchedule : BaseModel
    {
        public int EventId { get; set; }
        public Event Event { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public List<Attendance>? AttendancesEntry { get; set; }
        public List<Attendance>? AttendancesExit { get; set; }
        public string? QrCodeKey { get; set; }
    }
}

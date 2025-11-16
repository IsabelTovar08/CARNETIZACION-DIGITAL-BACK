using Entity.Models.Base;
using Entity.Models.Operational;
using System;

namespace Entity.Models.Organizational.Structure
{
    public class Schedule : GenericModel
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Days { get; set; }


        /// <summary>
        /// Conexion con la nueva tabla intermedia entre evento y schedule
        /// </summary>
        public ICollection<EventSchedule> EventSchedules { get; set; } = new List<EventSchedule>();

    }
}
    
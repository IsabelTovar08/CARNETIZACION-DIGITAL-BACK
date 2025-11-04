using System;
using Entity.Models.Base;

namespace Entity.Models.Organizational.Structure
{
    public class Schedule : GenericModel
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        //public int OrganizationId { get; set; }
        //public Organization? Organization { get; set; }
        
    }
}
    
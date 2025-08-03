using System;
using Entity.Models.Base;

namespace Entity.Models.Organizational.Structure
{
    public class Schedule : GenericModel
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public int OrganizationId { get; set; }
        public Organization? Organization { get; set; }
    }
}

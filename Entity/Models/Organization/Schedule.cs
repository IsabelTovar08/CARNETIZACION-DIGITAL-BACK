using System;
using Entity.Models.Base;

namespace Entity.Models.Organization
{
    public class Schedule : BaseModel
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Name { get; set; }

        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}

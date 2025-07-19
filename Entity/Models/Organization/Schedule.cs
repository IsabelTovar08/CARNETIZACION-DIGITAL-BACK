using System;
using Entity.Models.Base;

namespace Entity.Models.Organization
{
    public class Schedule : GenericModel
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}

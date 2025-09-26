using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Structure.Response
{
    public class ScheduleDto : GenericDto
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public int OrganizationId { get; set; }
        public string? OrganizationName { get; set; }

    }
}

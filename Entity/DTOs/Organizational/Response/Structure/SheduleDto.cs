using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Response.Structure
{
    public class SheduleDto : GenericBaseDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int OrganizationId { get; set; }
        public string? OrganizationName { get; set; }

    }
}

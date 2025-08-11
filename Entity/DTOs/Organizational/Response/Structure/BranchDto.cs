using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Response.Structure
{
    public class BranchDto : GenericBaseDto
    {
        public string? Location { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public int CityName { get; set; }


        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }


    }
}

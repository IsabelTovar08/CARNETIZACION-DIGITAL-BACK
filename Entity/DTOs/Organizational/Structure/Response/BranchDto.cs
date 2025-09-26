using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Structure.Response
{
    public class BranchDto : GenericDto
    {
        public string? Location { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public int CityId { get; set; }
        public string CityName { get; set; }            

        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }    


    }
}

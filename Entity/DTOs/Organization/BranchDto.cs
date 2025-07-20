using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organization
{
    public class BranchDto : GenericBaseDto
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; }

    }
}

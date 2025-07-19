using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organization
{
    public  class OrganizationalUnitBranchDto : GenericBaseDto
    {
        public int OrganizationalUnitId { get; set; }
        public string OrganizationalUnitName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }

    }
}

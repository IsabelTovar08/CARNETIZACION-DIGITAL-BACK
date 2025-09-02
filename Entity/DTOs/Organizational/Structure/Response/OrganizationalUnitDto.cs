using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Structure.Response
{
    public class OrganizationalUnitDto : GenericDto
    {
        public string? Description { get; set; }
        public int DivisionsCount { get; set; }
        public int BranchesCount { get; set; }

    }
}

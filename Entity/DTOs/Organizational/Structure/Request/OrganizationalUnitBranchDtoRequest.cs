using Entity.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Structure.Request
{
    public class OrganizationalUnitBranchDtoRequest : BaseModel
    {
        public int BranchId { get; set; }
        public string? BranchName { get; set; }

        public int OrganizationalUnitId { get; set; }
        public string? OrganizationalUnitName { get; set; }
    }
}

using Entity.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Request.Structure
{
    public class OrganizationalUnitDtoRequest : BaseModel
    {
        public string? Description { get; set; }

        public int BranchesCount { get; set; }
    }
}

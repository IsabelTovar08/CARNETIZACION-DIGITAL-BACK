using Entity.DTOs.Base;
using Entity.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Structure.Request
{
    public class OrganizationalUnitCreateWithBranchesDtoRequest : BaseModel
    {
        public OrganizationalUnitDtoRequest Unit { get; set; } = new();
        public List<int> BranchIds { get; set; } = new();
    }
}

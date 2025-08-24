using Entity.DTOs.Base;
using Entity.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Structure.Request
{
    public class InternalDivisionDtoRequest : GenericBaseDto
    {
        public string? Description { get; set; }
        public string OrganizationalUnitName { get; set; }
        public int AreaCategoryName { get; set; }
    }
}

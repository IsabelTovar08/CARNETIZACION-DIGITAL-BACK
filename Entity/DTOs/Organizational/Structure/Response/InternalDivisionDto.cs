using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Structure.Response
{
    public class InternalDivisionDto : GenericDto
    {
        public string? Description { get; set; }

        public int OrganizationalUnitId { get; set; }
        public string OrganizationalUnitName { get; set; }
        public int AreaCategoryId {  get; set; }
        public string AreaCategoryName { get; set; }

    }
}

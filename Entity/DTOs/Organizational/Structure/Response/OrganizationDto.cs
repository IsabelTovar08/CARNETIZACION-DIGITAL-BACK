using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Structure.Response
{
    public  class OrganizationDto : GenericDto
    {
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public int TypeId { get; set; }
        public string? TypeName { get; set; }

    }
}

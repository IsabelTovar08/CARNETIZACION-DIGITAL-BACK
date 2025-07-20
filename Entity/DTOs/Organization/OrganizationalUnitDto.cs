using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organization
{
    public class OrganizationalUnitDto : GenericBaseDto
    { 
        public int InternalDivisionId { get; set; }
        public string InternalDivisionName { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        
    }
}

using Entity.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Organizational.Structure.Request
{
    public class OrganizationalUnitDtoRequest : GenericModel
    {
        public string? Description { get; set; }

    }
}

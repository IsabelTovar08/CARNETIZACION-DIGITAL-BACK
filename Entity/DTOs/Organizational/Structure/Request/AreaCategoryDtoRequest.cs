using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Base;

namespace Entity.DTOs.Organizational.Structure.Request
{
    public class AreaCategoryDtoRequest : GenericModel
    {
        public string? Description { get; set; }
    }
}

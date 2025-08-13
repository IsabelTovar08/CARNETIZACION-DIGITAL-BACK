using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;

namespace Entity.DTOs.Parameter.Request
{
    public class CustomTypeRequest : GenericBaseDto
    {
        public string? Description { get; set; }
        public int TypeCategoryId { get; set; }
    }
}

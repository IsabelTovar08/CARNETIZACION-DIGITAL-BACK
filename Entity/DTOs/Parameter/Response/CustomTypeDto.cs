using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;
using Entity.Models.Base;
using Entity.Models.ModelSecurity;
using Entity.Models.Notifications;
using Entity.Models.Organizational.Structure;

namespace Entity.DTOs.Parameter.Response
{
    public class CustomTypeDto : GenericDto
    {
        public string? Description { get; set; }
        public int TypeCategoryId { get; set; }
        public string? TypeCategoryName { get; set; }


       
    }
}

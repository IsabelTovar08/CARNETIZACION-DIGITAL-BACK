using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;
using Entity.Models;
using Entity.Models.Base;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class MenuStructureRequest : BaseDTO
    {
        public int? ParentMenuId { get; set; }
        public int? ModuleId { get; set; }
        public int? FormId { get; set; }
        public string? Icon { get; set; }
        public string Title { get; set; }

        public string Type { get; set; }
        public int OrderIndex { get; set; }
    }
}

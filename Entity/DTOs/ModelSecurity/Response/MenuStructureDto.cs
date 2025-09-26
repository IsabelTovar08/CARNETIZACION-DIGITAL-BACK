using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Response
{
    public class MenuStructureDto : BaseDTO
    {
        public string Title { get; set; }            // Texto visible
        public string Type { get; set; }             // "group", "collapse", "item"
        public string? Classes { get; set; }         // Clases CSS
        public string? Url { get; set; }             // Enlace
        public string? Icon { get; set; }            // Icono

        public int? ParentMenuId { get; set; }
        public int? ModuleId { get; set; }
        public int? FormId { get; set; }

        // Hijos (estructura recursiva)
        public List<MenuStructureDto>? Children { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Entity.Models.Base;
using Entity.Models.Organizational.Structure;

namespace Entity.Models.ModelSecurity
{
    public class MenuStructure : BaseModel
    {
        public int? ParentMenuId { get; set; }
        public int? ModuleId { get; set; }   // dominio real 
        public int? FormId { get; set; }     // solo en item

        public string Type { get; set; }     // "group" | "collapse" | "item"
        public int OrderIndex { get; set; }

       // Solo para collapse(organización visual)
        public string? Title { get; set; }
        public string? Icon { get; set; }

        [JsonIgnore] 
        public MenuStructure Parent { get; set; }
        public ICollection<MenuStructure> Children { get; set; }
        public Module Module { get; set; }
        public Form Form { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Enums
{
    public class EnumOptionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Acronym { get; set; }  
        public string? Code { get; set; }      
    }
}

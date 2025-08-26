using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class FormDtoRequest : GenericDtoRequest
    {
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string Url { get; set; }
        public int ModuleId { get; set; }
    }
}

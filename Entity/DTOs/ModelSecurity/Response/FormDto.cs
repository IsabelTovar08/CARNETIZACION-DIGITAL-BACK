using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Response
{
    public class FormDto : GenericDto
    {
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string Url { get; set; }
        public int ModuleId { get; set; }
        public string? ModuleName { get; set; }

    }
}

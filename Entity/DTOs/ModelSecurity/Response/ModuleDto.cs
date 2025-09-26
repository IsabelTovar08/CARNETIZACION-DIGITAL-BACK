using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Response
{
    public class ModuleDto : GenericDto
    {
        public string? Description { get; set; }
        public string? Icon { get; set; }

    }
}

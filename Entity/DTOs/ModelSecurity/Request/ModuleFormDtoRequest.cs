using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class ModuleFormDtoRequest : BaseDTO
    {
        [Required]
        public int ModuleId { get; set; }
        [Required]
        public int FormId { get; set; }
    }
}

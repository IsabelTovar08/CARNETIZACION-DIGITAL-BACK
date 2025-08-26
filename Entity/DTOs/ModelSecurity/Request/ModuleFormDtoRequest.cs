using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class ModuleFormDtoRequest : BaseDtoRequest
    {
        [Required(ErrorMessage = "El identificador del módulo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del módulo debe ser un número entero mayor que 0.")]
        public int ModuleId { get; set; }

        [Required(ErrorMessage = "El identificador del formulario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del formulario debe ser un número entero mayor que 0.")]
        public int FormId { get; set; }
    }
}

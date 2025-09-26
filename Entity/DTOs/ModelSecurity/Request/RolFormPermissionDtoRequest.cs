using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class RolFormPermissionDtoRequest : BaseDtoRequest
    {
        [Required(ErrorMessage = "El identificador del rol es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del rol debe ser un número entero mayor que 0.")]
        public int RolId { get; set; }

        [Required(ErrorMessage = "El identificador del formulario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del formulario debe ser un número entero mayor que 0.")]
        public int FormId { get; set; }

        [Required(ErrorMessage = "El identificador del permiso es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del permiso debe ser un número entero mayor que 0.")]
        public int PermissionId { get; set; }
    }
}

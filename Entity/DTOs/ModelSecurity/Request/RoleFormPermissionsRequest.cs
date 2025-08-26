using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class RoleFormPermissionsRequest
    {
        [Required(ErrorMessage = "El identificador del rol es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del rol debe ser un número entero mayor que 0.")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "El identificador del formulario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del formulario debe ser un número entero mayor que 0.")]
        public int FormId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos un permiso.")]
        [MinLength(1, ErrorMessage = "Debe especificar al menos un permiso.")]
        public List<int> PermissionsIds { get; set; } = new List<int>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class UserRolesRequest
    {
        [Required(ErrorMessage = "El identificador del usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del usuario debe ser un número entero mayor que 0.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Debe asignar al menos un rol.")]
        [MinLength(1, ErrorMessage = "Debe especificar al menos un rol.")]
        public List<int> RolesId { get; set; } = new List<int>();
    }
}

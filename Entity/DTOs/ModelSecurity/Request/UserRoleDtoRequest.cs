using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class UserRoleDtoRequest : BaseDtoRequest
    {
        [Required(ErrorMessage = "El identificador del usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del usuario debe ser un número entero mayor que 0.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "El identificador del rol es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del rol debe ser un número entero mayor que 0.")]
        public int RolId { get; set; }
    }
}

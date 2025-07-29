
using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class UserRoleDtoRequest : BaseDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RolId { get; set; }
    }
}

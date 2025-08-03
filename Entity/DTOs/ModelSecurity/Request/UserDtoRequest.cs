
using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class UserDtoRequest : BaseDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }

        [Required]
        public int PersonId { get; set; }
    }
}

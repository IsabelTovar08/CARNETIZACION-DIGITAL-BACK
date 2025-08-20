
using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class UserDtoRequest : BaseDTO
    {
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public int PersonId { get; set; }
    }
}

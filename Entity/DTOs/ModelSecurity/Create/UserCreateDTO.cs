
using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs.Create
{
    public class UserCreateDTO
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }

        [Required]
        public int PersonId { get; set; }
    }
}

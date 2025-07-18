
using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.Create
{
    public class UserRolCreateDto : BaseDTO
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RolId { get; set; }
    }
}

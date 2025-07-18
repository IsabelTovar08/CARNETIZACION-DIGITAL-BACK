
using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs.Create
{
    public class RolFormPermissionCreateDto
    {
        public int Id { get; set; }
        [Required]
        public int RolId { get; set; }
        [Required]
        public int FormId { get; set; }
        [Required]
        public int PermissionId { get; set; }

    }
}

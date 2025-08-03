
using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class RolFormPermissionDtoRequest : BaseDTO
    {
        [Required]
        public int RolId { get; set; }
        [Required]
        public int FormId { get; set; }
        [Required]
        public int PermissionId { get; set; }

    }
}

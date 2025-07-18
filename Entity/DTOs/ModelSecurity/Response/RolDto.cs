using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs
{
    public class RolDto : BaseDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs
{
    public class ModuleDto : BaseDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}

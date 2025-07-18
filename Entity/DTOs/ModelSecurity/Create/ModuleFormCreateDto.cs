using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs.Create
{
    public class ModuleFormCreateDto
    {
        public int Id { get; set; }
        [Required]
        public int ModuleId { get; set; }
        [Required]
        public int FormId { get; set; }
    }
}

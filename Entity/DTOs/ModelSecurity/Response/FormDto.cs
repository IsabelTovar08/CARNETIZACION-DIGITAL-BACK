using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs
{
    public class FormDto : BaseDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Url { get; set; }

    }
}

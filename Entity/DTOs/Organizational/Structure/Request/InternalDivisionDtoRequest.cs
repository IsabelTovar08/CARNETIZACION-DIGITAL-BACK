using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.Organizational.Structure.Request
{
    public class InternalDivisionDtoRequest : GenericDtoRequest
    {
        [StringLength(300, ErrorMessage = "La descripción no puede exceder los 300 caracteres.")]
        [RegularExpression(@"(^$|.*\S.*)", ErrorMessage = "La descripción no puede estar compuesta únicamente por espacios en blanco.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "El identificador de la unidad organizacional es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la unidad organizacional debe ser un número entero mayor que 0.")]
        public int OrganizationalUnitId { get; set; }

        [Required(ErrorMessage = "El identificador de la categoría del área es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la categoría del área debe ser un número entero mayor que 0.")]
        public int AreaCategoryId { get; set; }
    }
}

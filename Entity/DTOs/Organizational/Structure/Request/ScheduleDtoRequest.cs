using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.Organizational.Structure.Request
{
    public class ScheduleDtoRequest : GenericDtoRequest
    {
        [StringLength(300, ErrorMessage = "La descripción no puede exceder los 300 caracteres.")]
        [RegularExpression(@"(^$|.*\S.*)", ErrorMessage = "La descripción no puede estar compuesta únicamente por espacios en blanco.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        public TimeOnly StartTime { get; set; }

        [Required(ErrorMessage = "La hora de finalización es obligatoria.")]
        public TimeOnly EndTime { get; set; }

        [Required(ErrorMessage = "El identificador de la organización es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la organización debe ser un número entero mayor que 0.")]
        public int OrganizationId { get; set; }
    }
}

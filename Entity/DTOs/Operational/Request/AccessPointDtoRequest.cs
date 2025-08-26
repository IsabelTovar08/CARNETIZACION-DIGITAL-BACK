using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.Operational.Request
{
    public class AccessPointDtoRequest : GenericDtoRequest
    {
        [StringLength(300, ErrorMessage = "La descripción no puede exceder los 300 caracteres.")]
        [RegularExpression(@"(^$|.*\S.*)", ErrorMessage = "La descripción no puede estar compuesta únicamente por espacios en blanco.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "El identificador del evento es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del evento debe ser un número entero mayor que 0.")]
        public int EventId { get; set; }

        [Required(ErrorMessage = "El identificador del tipo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del tipo debe ser un número entero mayor que 0.")]
        public int TypeId { get; set; }
    }
}

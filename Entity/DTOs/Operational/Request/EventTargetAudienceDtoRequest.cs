using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.Operational.Request
{
    public class EventTargetAudienceDtoRequest : GenericDtoRequest
    {
        [Required(ErrorMessage = "El identificador del tipo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del tipo debe ser un número entero mayor que 0.")]
        public int TypeId { get; set; }

        [Required(ErrorMessage = "El identificador de la referencia es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la referencia debe ser un número entero mayor que 0.")]
        public int ReferenceId { get; set; }

        [Required(ErrorMessage = "El identificador del evento es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del evento debe ser un número entero mayor que 0.")]
        public int EventId { get; set; }
    }
}

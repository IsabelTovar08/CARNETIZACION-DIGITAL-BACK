using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.Organizational.Location.Request
{
    public class CityDtoRequest : GenericDtoRequest
    {
        [Required(ErrorMessage = "El identificador del departamento es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del departamento debe ser un número entero mayor que 0.")]
        public int DeparmentId { get; set; }
    }
}

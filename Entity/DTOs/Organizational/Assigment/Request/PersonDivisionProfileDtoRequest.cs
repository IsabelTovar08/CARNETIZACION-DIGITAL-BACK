using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.Organizational.Assigment.Request
{
    public class PersonDivisionProfileDtoRequest : BaseDtoRequest
    {
        [Required(ErrorMessage = "El identificador de la persona es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la persona debe ser un número entero mayor que 0.")]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "El identificador de la división interna es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la división interna debe ser un número entero mayor que 0.")]
        public int InternalDivisionId { get; set; }

        [Required(ErrorMessage = "El identificador del perfil es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del perfil debe ser un número entero mayor que 0.")]
        public int ProfileId { get; set; }
    }
}

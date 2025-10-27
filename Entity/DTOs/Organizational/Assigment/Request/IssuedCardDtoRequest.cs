using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;
using Entity.Models.Organizational.Assignment;
using Entity.Models.Parameter;

namespace Entity.DTOs.Organizational.Assigment.Request
{
    public class IssuedCardDtoRequest : BaseDtoRequest
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
        public bool isCurrentlySelected { get; set; }
        public string? PdfUrl { get; set; }
        public int CardId { get; set; }
        public CardConfiguration Card { get; set; }
        public int StatusId { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.Organizational.Assigment.Request
{
    public class CardDtoRequest : GenericDtoRequest
    {
        [Required(ErrorMessage = "La fecha de creación es obligatoria.")]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "La fecha de expiración es obligatoria.")]
        public DateTime ExpirationDate { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del estado debe ser un número entero mayor que 0.")]
        public int StatusId { get; set; }

        [Required(ErrorMessage = "El identificador de la asignación persona–división–perfil es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la asignación persona–división–perfil debe ser un número entero mayor que 0.")]
        public int PersonDivissionProfileId { get; set; }
    }
}

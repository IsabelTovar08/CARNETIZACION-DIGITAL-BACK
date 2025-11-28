using System;
using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.Organizational.Assigment.Request
{
    public class CardConfigurationDtoRequest : GenericDtoRequest
    {
        [Required(ErrorMessage = "La fecha de creación es obligatoria.")]
        public DateTime ValidFrom { get; set; }

        [Required(ErrorMessage = "La fecha de expiración es obligatoria.")]
        public DateTime ValidTo { get; set; }


        [Required(ErrorMessage = "El identificador de la asignación persona–división–perfil es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la asignación persona–división–perfil debe ser un número entero mayor que 0.")]
        public int ProfileId { get; set; }

        [Required(ErrorMessage = "El identificador de la asignación card-template es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la asignación card-template debe ser un número entero mayor que 0.")]
        public int CardTemplateId { get; set; }

    }
}

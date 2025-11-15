using System;
using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.Operational.Request
{
    public class EventDtoRequest : GenericDtoRequest
    {
        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El código debe tener entre 3 y 50 caracteres.")]
        [RegularExpression(@"^[A-Za-z0-9._\-]+$", ErrorMessage = "El código solo puede contener letras, números, guiones, guiones bajos y puntos.")]
        public string Code { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "La descripción no puede exceder los 300 caracteres.")]
        [RegularExpression(@"(^$|.*\S.*)", ErrorMessage = "La descripción no puede estar compuesta únicamente por espacios en blanco.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "La fecha de inicio del evento es obligatoria.")]
        public DateTime? EventStart { get; set; }

        [Required(ErrorMessage = "La fecha de fin del evento es obligatoria.")]
        public DateTime? EventEnd{ get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El identificador del horario debe ser un número entero mayor que 0.")]
        public int? ScheduleId { get; set; }

        [Required(ErrorMessage = "El tipo de evento es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El tipo de evento debe ser un número entero mayor que 0.")]
        public int EventTypeId { get; set; }

        public String? EventName { get; set; }

        [Required(ErrorMessage = "Debe especificar si el evento es público o no.")]
        public bool Ispublic { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El estado debe ser un número entero mayor que 0.")]
        public int StatusId { get; set; }
        //public List<string>? Days { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos una jornada.")]
        public List<int> ScheduleIds { get; set; } = new();
        public List<int>? AccessPoints { get; set; } = new();
        public List<int>? ProfileIds { get; set; } = new();
        public List<int>? OrganizationalUnitIds { get; set; } = new();
        public List<int>? InternalDivisionIds { get; set; } = new();

    }
}

using System;
using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.Operational.Request
{
    public class AttendanceDtoRequestSpecific : BaseDtoRequest
    {
        [Required(ErrorMessage = "El identificador de la persona es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la persona debe ser un número entero mayor que 0.")]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "La hora de registro es obligatoria.")]
        public DateTime Time { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El identificador del punto de acceso debe ser un número entero mayor que 0.")]
        public int? AccessPoint { get; set; }
    }
}

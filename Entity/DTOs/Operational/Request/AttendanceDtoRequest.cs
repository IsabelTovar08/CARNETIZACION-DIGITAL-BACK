using System;
using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.Operational.Request
{
    public class AttendanceDtoRequest : BaseDtoRequest
    {
        [Required(ErrorMessage = "El identificador de la persona es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la persona debe ser un número entero mayor que 0.")]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "La hora de registro de entrada es obligatoria.")]
        public DateTime TimeOfEntry { get; set; }

        public DateTime? TimeOfExit { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El identificador del punto de acceso de entrada debe ser un número entero mayor que 0.")]
        public int? AccessPointOfEntry { get; set; }

        public int? AccessPointOfExit { get; set; }

        /// <summary>
        /// QR recibido desde el frontend para registrar asistencia.
        /// </summary>
        public string? QrCode { get; set; }
    }
}

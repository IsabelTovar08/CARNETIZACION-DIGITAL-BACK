using System;
using System.ComponentModel.DataAnnotations;
using Entity.DTOs.Base;

namespace Entity.DTOs.Operational.Request
{
    public class AttendanceDtoRequestSpecific : BaseDtoRequest
    {
        /// <summary>
        /// Se asigna automáticamente desde el token (no es obligatorio en la solicitud).
        /// </summary>
        public int PersonId { get; set; } // ✅ nullable para que Swagger no valide antes de tiempo

        /// <summary>
        /// Hora de registro (opcional, normalmente asignada por backend).
        /// </summary>
        public DateTime? Time { get; set; } // ✅ también opcional, el backend usa DateTime.UtcNow

        /// <summary>
        /// Identificador del punto de acceso donde se registra la asistencia.
        /// </summary>
        public string? QrCodeKey { get; set; } = default!;
    }
}

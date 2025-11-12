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
        public int? PersonId { get; set; } // ✅ nullable para que Swagger no valide antes de tiempo

        /// <summary>
        /// Hora de registro (opcional, normalmente asignada por backend).
        /// </summary>
        public DateTime? Time { get; set; } // ✅ también opcional, el backend usa DateTime.UtcNow

        /// <summary>
        /// Identificador del punto de acceso donde se registra la asistencia.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del punto de acceso debe ser un número entero mayor que 0.")]
        public int? AccessPoint { get; set; }

        /// <summary>
        /// Alias de AccessPoint para compatibilidad con lógica existente.
        /// </summary>
        public int? AccessPointId
        {
            get => AccessPoint;
            set => AccessPoint = value;
        }

        /// <summary>
        /// ✅ Nuevo: identificador del evento al que pertenece la asistencia.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del evento debe ser un número entero mayor que 0.")]
        public int? EventId { get; set; }
    }
}

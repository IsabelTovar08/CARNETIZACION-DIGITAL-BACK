using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics.Attendances
{
    /// <summary>
    /// DTO que representa la última asistencia por persona dentro de un evento.
    /// Incluye puntos de acceso de entrada y salida.
    /// </summary>
    public class AttendanceLiteDto
    {
        public int AttendanceId { get; set; }
        public int PersonId { get; set; }

        public DateTime TimeOfEntry { get; set; }
        public DateTime? TimeOfExit { get; set; }

        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty;

        public string AccessPointEntryName { get; set; } = string.Empty;
        public string? AccessPointExitName { get; set; }

        public bool HasMoreAttendances { get; set; }
    }

}

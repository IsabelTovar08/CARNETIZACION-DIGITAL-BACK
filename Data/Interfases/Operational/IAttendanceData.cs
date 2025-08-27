using System;
using System.Threading;
using System.Threading.Tasks;
using Data.Interfases;
using Entity.Models.Organizational;

namespace Data.Interfases.Operational
{
    public interface IAttendanceData : ICrudBase<Attendance>
    {
        /// <summary>
        /// Devuelve la última asistencia abierta (TimeOfExit == null) para la persona, o null si no hay.
        /// </summary>
        Task<Attendance?> GetOpenAttendanceAsync(int personId, CancellationToken ct = default);

        /// <summary>
        /// Actualiza la salida (TimeOfExit y AccessPointOfExit) del registro con Id dado.
        /// Retorna la entidad actualizada.
        /// </summary>
        Task<Attendance> UpdateExitAsync(int id, DateTime timeOfExit, int? accessPointOut, CancellationToken ct = default);
    }
}

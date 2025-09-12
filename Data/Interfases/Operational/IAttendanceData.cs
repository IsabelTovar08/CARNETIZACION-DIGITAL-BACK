using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Consulta filtrada y paginada de asistencias.
        /// Filtros opcionales: personId, eventId (por AccessPoint Entry/Exit), rango de fechas, orden y paginación.
        /// </summary>
        Task<(IList<Attendance> Items, int Total)> QueryAsync(
            int? personId, int? eventId, DateTime? fromUtc, DateTime? toUtc,
            string? sortBy, string? sortDir, int page, int pageSize,
            CancellationToken ct = default);
    }
}

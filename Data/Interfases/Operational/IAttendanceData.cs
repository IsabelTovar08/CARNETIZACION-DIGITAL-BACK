using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Interfases;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Specifics.Attendances;
using Entity.Models.Organizational;

namespace Data.Interfases.Operational
{
    public interface IAttendanceData : ICrudBase<Attendance>
    {
        /// <summary>
        /// Devuelve la última asistencia abierta (TimeOfExit == null) para la persona, o null si no hay.
        /// </summary>
        Task<Attendance?> GetOpenAttendanceAsync(int personId, int eventAccessPointEntryId);


        /// <summary>
        /// Consulta filtrada y paginada de asistencias.
        /// </summary>
        Task<(IList<Attendance> Items, int Total)> QueryAsync(
            int? personId, int? eventId, DateTime? fromUtc, DateTime? toUtc,
            string? sortBy, string? sortDir, int page, int pageSize,
            CancellationToken ct = default);

        // NUEVO: Reporte filtrado sin paginación
        //Task<IList<Attendance>> GetReportAsync(
        //    int? eventId, int? personId, DateTime? startDate, DateTime? endDate,
        //    CancellationToken ct = default);

        // ✅ NUEVO MÉTODO: necesario para que el Business pueda hacer Include()
        IQueryable<Attendance> GetQueryable();

        /// <summary>
        /// Consultar todas las asistencias de una persona a un evento específico.
        /// </summary>
        Task<IList<AttendanceDtoResponse>> GetAllByPersonEventAsync(int personId, int eventId);

        /// <summary>
        /// Consultar si una persona tiene más de una asistencia en un evento específico
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="eventId"></param>
        /// <param name="currentAttendanceId"></param>
        /// <returns></returns>
        Task<bool> PersonHasMoreAttendancesAsync(int personId, int eventId, int currentAttendanceId);
    }
}

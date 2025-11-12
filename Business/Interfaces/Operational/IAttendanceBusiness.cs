using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Reports;
using Entity.Models.Organizational;

namespace Business.Interfaces.Operational
{
    public interface IAttendanceBusiness : IBaseBusiness<Attendance, AttendanceDtoRequest, AttendanceDtoResponse>
    {
        /// <summary>
        /// ✅ Registra asistencia general (sin entrada/salida específica)
        /// </summary>
        Task<AttendanceDtoResponse?> RegisterAttendanceAsync(AttendanceDtoRequest dto);

        /// <summary>
        /// ✅ Registra entrada manual (desde token)
        /// </summary>
        Task<AttendanceDtoResponse> RegisterEntryAsync(AttendanceDtoRequestSpecific dto, CancellationToken ct = default);

        /// <summary>
        /// ✅ Registra salida manual (desde token)
        /// </summary>
        Task<AttendanceDtoResponse> RegisterExitAsync(AttendanceDtoRequestSpecific dto, CancellationToken ct = default);

        /// <summary>
        /// ✅ Registra asistencia automáticamente al leer un código QR del evento.
        /// QR esperado: EVT|EventId|EventName|AccessPointName
        /// </summary>
        Task<AttendanceDtoResponse> RegisterAttendanceByQrAsync(string qrContent, int personId);

        /// <summary>
        /// ✅ Búsqueda filtrada de asistencias
        /// </summary>
        Task<(IList<AttendanceDtoResponse> Items, int Total)> SearchAsync(
            int? personId,
            int? eventId,
            DateTime? fromUtc,
            DateTime? toUtc,
            string? sortBy,
            string? sortDir,
            int page,
            int pageSize,
            CancellationToken ct = default
        );

        /// <summary>
        /// ✅ Exporta las asistencias a PDF
        /// </summary>
        Task<byte[]> ExportToPdfAsync(IEnumerable<AttendanceDtoResponse> data, CancellationToken ct = default);

        /// <summary>
        /// ✅ Exporta las asistencias a Excel
        /// </summary>
        Task<byte[]> ExportToExcelAsync(IEnumerable<AttendanceDtoResponse> data, CancellationToken ct = default);
    }
}

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
        Task<AttendanceDtoResponse?> RegisterAttendanceAsync(AttendanceDtoRequest dto);
        Task<AttendanceDtoResponse> RegisterEntryAsync(AttendanceDtoRequestSpecific dto, CancellationToken ct = default);
        Task<AttendanceDtoResponse> RegisterExitAsync(AttendanceDtoRequestSpecific dto, CancellationToken ct = default);

        Task<(IList<AttendanceDtoResponse> Items, int Total)> SearchAsync(
            int? personId, int? eventId, DateTime? fromUtc, DateTime? toUtc,
            string? sortBy, string? sortDir, int page, int pageSize,
            CancellationToken ct = default);

        // <sumary>
        /// pdf y excel export
        /// </sumary>

        Task<byte[]> ExportToPdfAsync(IEnumerable<AttendanceDtoResponse> data, CancellationToken ct = default);
        Task<byte[]> ExportToExcelAsync(IEnumerable<AttendanceDtoResponse> data, CancellationToken ct = default);

    }
}

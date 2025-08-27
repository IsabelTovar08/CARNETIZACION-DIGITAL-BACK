using System.Threading;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;

namespace Business.Interfaces.Operational
{
    public interface IAttendanceBusiness : IBaseBusiness<Attendance, AttendanceDtoRequest, AttendanceDtoResponse>
    {
        /// <summary>Registra asistencia con el DTO general (scan actual que ya tenías).</summary>
        Task<AttendanceDtoResponse?> RegisterAttendanceAsync(AttendanceDtoRequest dto);

        /// <summary>Registra SOLO la ENTRADA. Falla si ya existe una entrada abierta.</summary>
        Task<AttendanceDtoResponse> RegisterEntryAsync(AttendanceDtoRequestSpecific dto, CancellationToken ct = default);

        /// <summary>Registra SOLO la SALIDA. Falla si no existe una entrada abierta.</summary>
        Task<AttendanceDtoResponse> RegisterExitAsync(AttendanceDtoRequestSpecific dto, CancellationToken ct = default);
    }
}

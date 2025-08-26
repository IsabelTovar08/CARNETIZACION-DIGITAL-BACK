using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;

namespace Business.Interfaces.Operational
{
    public interface IAttendanceBusiness : IBaseBusiness<Attendance, AttendanceDtoRequest, AttendanceDtoResponse>
    {
        /// <summary>
        /// Registra la asistencia a través de un QR escaneado.
        /// </summary>
        Task<AttendanceDtoResponse?> RegisterAttendanceAsync(AttendanceDtoRequest dto);
    }
}

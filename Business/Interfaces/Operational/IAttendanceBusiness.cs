using Business.Interfases;
using Entity.DTOs.Operational;
using Entity.Models.Organizational;
using System.Threading.Tasks;

namespace Business.Interfaces.Operational
{
    public interface IAttendanceBusiness : IBaseBusiness<Attendance, AttendanceDto, AttendanceDto>
    {
        /// <summary>
        /// Registra la asistencia a través de un QR escaneado.
        /// </summary>
        Task<AttendanceDto?> RegisterAttendanceAsync(AttendanceDto dto);
    }
}

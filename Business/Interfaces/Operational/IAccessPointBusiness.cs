using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;

namespace Business.Interfaces.Operational
{
    public interface IAccessPointBusiness : IBaseBusiness<AccessPoint, AccessPointDtoRequest, AccessPointDtoResponsee>
    {
        Task<AccessPointDtoResponsee?> RegisterAsync(AccessPointDtoRequest dto);

        /// <summary>
        /// Registrar asistencia mediante escaneo de QR.
        /// </summary>
        Task<AttendanceDtoResponse?> RegisterAttendanceByQrAsync(string qrCode, int personId);
    }
}

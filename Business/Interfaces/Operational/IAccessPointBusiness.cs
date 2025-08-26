using Business.Interfases;
using Entity.DTOs.Operational;
using Entity.Models.Organizational;
using System.Threading.Tasks;

namespace Business.Interfaces.Operational
{
    public interface IAccessPointBusiness : IBaseBusiness<AccessPoint, AccessPointDto, AccessPointDto>
    {
        /// <summary>
        /// Crea el punto de acceso y genera su QR (Base64 PNG).
        /// </summary>
        Task<AccessPointDto?> RegisterAsync(AccessPointDto dto);
    }
}

using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;

namespace Business.Interfaces.Operational
{
    public interface IAccessPointBusiness : IBaseBusiness<AccessPoint, AccessPointDtoRequest, AccessPointDtoResponsee>
    {
        /// <summary>
        /// Crea el punto de acceso y genera su QR (Base64 PNG).
        /// </summary>
        Task<AccessPointDtoResponsee?> RegisterAsync(AccessPointDtoRequest dto);
    }
}

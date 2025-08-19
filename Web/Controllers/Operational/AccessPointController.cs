using Business.Interfaces.Operational;
using Business.Interfases;
using Entity.DTOs.Operational;
using Entity.Models.Organizational;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    public class AccessPointController : GenericController<AccessPoint, AccessPointDto, AccessPointDto>
    {
        public AccessPointController(IAccessPointBusiness business, ILogger<AccessPointController> logger) : base(business, logger)
        {
        }
    }
}

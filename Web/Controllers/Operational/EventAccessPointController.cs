using Business.Interfaces.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    public class EventAccessPointController : GenericController<EventAccessPoint, EventAccessPointDtoRequest, EventAccessPointDto>
    {
        public EventAccessPointController(IEventAccessPointBusiness business, ILogger<EventAccessPointController> logger) : base(business, logger)
        {
        }
    }
}

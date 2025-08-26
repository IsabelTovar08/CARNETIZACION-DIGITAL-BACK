using Business.Interfaces.Operational;
using Business.Interfases;
using Entity.DTOs.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    public class EventTypeController : GenericController<EventType, EventTypeDtoRequest, EventTypeDtoResponse>
    {
        public EventTypeController(IEventTypeBusiness business, ILogger<EventTypeController> logger) : base(business, logger)
        {
        }
    }
}

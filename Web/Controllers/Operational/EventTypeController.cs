using Business.Interfaces.Operational;
using Business.Interfases;
using Entity.DTOs.Operational;
using Entity.Models.Organizational;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    public class EventTypeController : GenericController<EventType, EventTypeDto, EventTypeDto>
    {
        public EventTypeController(IEventTypeBusiness business, ILogger<EventTypeController> logger) : base(business, logger)
        {
        }
    }
}

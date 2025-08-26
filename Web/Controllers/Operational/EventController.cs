using Business.Interfaces.Operational;
using Business.Interfases;
using Entity.DTOs.Operational;
using Entity.Models.Organizational;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    public class EventController : GenericController<Event, EventDtoResponse, EventDtoResponse>
    {
        public EventController(IEventBusiness business, ILogger<EventController> logger) : base(business, logger)
        {
        }
    }
}

using Business.Interfaces.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    public class EventScheduleController : GenericController<EventSchedule, EventScheduleDtoRequest, EventScheduleDtoResponse>
    {
        public EventScheduleController(IEventScheduleBusiness business, ILogger<EventSchedule> logger) : base(business, logger)
        {
        }
    }
}

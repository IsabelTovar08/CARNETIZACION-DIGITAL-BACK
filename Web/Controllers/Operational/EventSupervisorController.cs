using Business.Interfaces.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    public class EventSupervisorController : GenericController<EventSupervisor, EventSupervisorDtoRequest, EventSupervisorDtoResponse>
    {
        public EventSupervisorController(IEventSupervisorBusiness business, ILogger<EventSupervisor> logger) : base(business, logger)
        {
        }
    }
}

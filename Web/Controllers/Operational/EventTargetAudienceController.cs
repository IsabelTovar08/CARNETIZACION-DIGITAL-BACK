using Business.Interfaces.Operational;
using Business.Interfases;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    public class EventTargetAudienceController : GenericController<EventTargetAudience, EventTargetAudienceDtoRequest, EventTargetAudienceDtoResponse>
    {
        public EventTargetAudienceController(IEventTargetAudienceBusiness business, ILogger<EventTargetAudienceController> logger) : base(business, logger)
        {
        }
    }
}

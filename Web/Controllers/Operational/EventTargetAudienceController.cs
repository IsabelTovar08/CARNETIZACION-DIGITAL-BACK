using Business.Interfaces.Operational;
using Business.Interfases;
using Entity.DTOs.Operational;
using Entity.Models.Organizational;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    public class EventTargetAudienceController : GenericController<EventTargetAudience, EventTargetAudienceDtoRequest, EventTargetAudienceDtoResponse>
    {
        public EventTargetAudienceController(IBaseBusiness<EventTargetAudience, EventTargetAudienceDtoRequest, EventTargetAudienceDtoResponse> business, ILogger<EventTargetAudienceController> logger)
            : base(business, logger)
        {
        }
    }
}

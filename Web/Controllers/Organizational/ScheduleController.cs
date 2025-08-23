using Business.Interfases;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models.Organizational.Structure;
using Web.Controllers.Base;

namespace Web.Controllers.Organizational
{
    public class ScheduleController : GenericController<Schedule, ScheduleDtoRequest, ScheduleDto>
    {
        public ScheduleController(IBaseBusiness<Schedule, ScheduleDtoRequest, ScheduleDto> business, ILogger<ScheduleController> logger) : base(business, logger)
        {
        }
    }
}

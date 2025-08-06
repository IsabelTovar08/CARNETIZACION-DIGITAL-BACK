using Business.Interfases;
using Entity.DTOs.Parameter;
using Entity.Models.Parameter;
using Web.Controllers.Base;

namespace Web.Controllers.Parameters
{
    public class StatusController : GenericController<Status, StatusDto, StatusDto>
    {
        private readonly IBaseBusiness<Status, StatusDto, StatusDto> _business;
        public StatusController(IBaseBusiness<Status, StatusDto, StatusDto> business, ILogger<StatusController> logger) : base(business, logger)
        {
            _business = business;
        }
    }
}

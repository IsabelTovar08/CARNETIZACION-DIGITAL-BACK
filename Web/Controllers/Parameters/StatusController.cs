using Business.Interfases;
using Entity.DTOs.Parameter;
using Entity.DTOs.Parameter.Request;
using Entity.DTOs.Parameter.Response;
using Entity.Models.Parameter;
using Web.Controllers.Base;

namespace Web.Controllers.Parameters
{
    public class StatusController : GenericController<Status, StatusDtoRequest, StatusDtoResponse>
    {
        private readonly IBaseBusiness<Status, StatusDtoRequest, StatusDtoResponse> _business;
        public StatusController(IBaseBusiness<Status, StatusDtoRequest, StatusDtoResponse> business, ILogger<StatusController> logger) : base(business, logger)
        {
            _business = business;
        }
    }
}

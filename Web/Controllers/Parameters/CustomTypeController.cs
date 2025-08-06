using Business.Interfases;
using Entity.DTOs.Parameter;
using Entity.Models.Parameter;
using Web.Controllers.Base;

namespace Web.Controllers.Parameters
{
    public class CustomTypeController : GenericController<CustomType, CustomTypeDto, CustomTypeDto>
    {
        private readonly IBaseBusiness<CustomType, CustomTypeDto, CustomTypeDto> _business;
        public CustomTypeController(IBaseBusiness<CustomType, CustomTypeDto, CustomTypeDto> business, ILogger<CustomTypeController> logger) : base(business, logger)
        {
            _business = business;
        }
    }
}

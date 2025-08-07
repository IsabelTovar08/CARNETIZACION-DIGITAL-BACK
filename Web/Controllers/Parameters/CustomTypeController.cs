using Business.Interfases;
using Entity.DTOs.Parameter;
using Entity.Models.Parameter;
using Web.Controllers.Base;

namespace Web.Controllers.Parameters
{
    public class CustomTypeController : GenericController<CustomType, CustomTypeDto, CustomTypeDto>
    {
        public CustomTypeController(IBaseBusiness<CustomType, CustomTypeDto, CustomTypeDto> business, ILogger<CustomTypeController> logger) : base(business, logger)
        {
        }
    }
}

using Business.Interfases;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models;
using Entity.Models.Organizational.Structure;
using Web.Controllers.Base;
using Web.Controllers.ModelSecurity;

namespace Web.Controllers.Organizational.Structure
{
    public class AreaCategoryController : GenericController<AreaCategory, AreaCategoryDtoRequest, AreaCategoryDto>
    {
        public AreaCategoryController(IBaseBusiness<AreaCategory, AreaCategoryDtoRequest, AreaCategoryDto> business, ILogger<AreaCategoryController> logger) : base(business, logger)
        {
        }
    }
}

using Business.Interfases;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models.Organizational.Structure;
using Web.Controllers.Base;

namespace Web.Controllers.Organizational.Structure
{
    public class BranchController : GenericController<Branch, BranchDtoRequest, BranchDto>
    {
        public BranchController(IBaseBusiness<Branch, BranchDtoRequest, BranchDto> business, ILogger<AreaCategoryController> logger) : base(business, logger) 
        {
        }
    }
}

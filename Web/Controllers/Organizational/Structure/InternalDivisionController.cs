using Business.Interfases;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models.Organizational.Structure;
using Web.Controllers.Base;

namespace Web.Controllers.Organizational.Structure
{
    public class InternalDivisionController : GenericController<InternalDivision, InternalDivisionDtoRequest, InternalDivisionDto>
    {
        public InternalDivisionController(IBaseBusiness<InternalDivision, InternalDivisionDtoRequest, InternalDivisionDto> business, ILogger<InternalDivisionController> logger) : base(business, logger)
        {
        }
    }
}

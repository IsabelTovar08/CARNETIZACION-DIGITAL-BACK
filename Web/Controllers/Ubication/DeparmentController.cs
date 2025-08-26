using Business.Interfases;
using Entity.DTOs.Organizational.Location.Response;
using Entity.Models.Organizational.Location;
using Web.Controllers.Base;

namespace Web.Controllers.Ubication
{
    public class DeparmentController : GenericController<Department, DepartmentDtoResponse, DepartmentDtoResponse>
    {

        public DeparmentController(IBaseBusiness<Department, DepartmentDtoResponse, DepartmentDtoResponse> business, ILogger<DeparmentController> logger) : base(business, logger)
        {
        }
    }
}

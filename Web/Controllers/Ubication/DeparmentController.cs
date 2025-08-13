using Business.Interfases;
using Entity.DTOs.Organizational.Response.Location;
using Entity.Models.Organizational.Location;
using Web.Controllers.Base;

namespace Web.Controllers.Ubication
{
    public class DeparmentController : GenericController<Department, DepartmentDto, DepartmentDto>
    {

        public DeparmentController(IBaseBusiness<Department, DepartmentDto, DepartmentDto> business, ILogger<DeparmentController> logger) : base(business, logger)
        {
        }
    }
}

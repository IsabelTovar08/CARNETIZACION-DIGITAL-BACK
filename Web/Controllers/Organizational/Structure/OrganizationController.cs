using Business.Interfaces.Organizational.Structure;
using Business.Interfases;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models.Organizational.Structure;
using Web.Controllers.Base;

namespace Web.Controllers.Organizational.Structure
{
    public class OrganizationController : GenericController<Organization, OrganizationDtoRequest, OrganizationDto>
    {
        public OrganizationController(IOrganizationBusiness business, ILogger<OrganizationController> logger) : base(business, logger)
        {
        }
    }
}

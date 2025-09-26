using Business.Interfases;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models.Organizational.Structure;
using Web.Controllers.Base;

namespace Web.Controllers.Organizational.Structure
{
    public class OrganizationalUnitBranchController : GenericController<OrganizationalUnitBranch, OrganizationalUnitBranchDtoRequest, OrganizationalUnitBranchDto>
    {
        public OrganizationalUnitBranchController(IBaseBusiness<OrganizationalUnitBranch, OrganizationalUnitBranchDtoRequest, OrganizationalUnitBranchDto> business, ILogger<OrganizationalUnitBranchController> logger) : base(business, logger)
        {
        }
    }
}

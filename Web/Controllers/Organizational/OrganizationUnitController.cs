using Business.Implementations.Organization;
using Business.Interfases;
using Entity.DTOs.Organizational.Request.Structure;
using Entity.DTOs.Organizational.Response.Structure;
using Entity.Models.Organizational.Structure;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

public class OrganizationalUnitController
  : GenericController<OrganizationalUnit, OrganizationalUnitDtoRequest, OrganizationalUnitDto>
{
    private readonly OrganizationalUnitBusiness _orgUnitBusiness;
    public OrganizationalUnitController(
        IBaseBusiness<OrganizationalUnit, OrganizationalUnitDtoRequest, OrganizationalUnitDto> business,
        ILogger<OrganizationalUnitController> logger,
        OrganizationalUnitBusiness orgUnitBusiness)
        : base(business, logger)
    {
        _orgUnitBusiness = orgUnitBusiness;
    }

    [HttpGet("{id}/divisions/count")]
    public async Task<IActionResult> GetDivisionsCount(int id)
    {
        if (id <= 0) return BadRequest(new { message = "Id inválido" });
        var count = await _orgUnitBusiness.CountDivisionAsync(id);
        return Ok(count);
    }
    [HttpGet("{id}/branches/count")]
    public async Task<IActionResult> GetBranchesCount(int id)
    {
        if (id <= 0) return BadRequest(new { message = "Id inválido" });
        var count = await _orgUnitBusiness.CountBranchesAsync(id);
        return Ok(count);
    }
}

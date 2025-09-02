using Business.Implementations.Organizational.Structure;
using Business.Interfases;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
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

    [HttpGet("{organizationalUnitId:int}/internal-divisions")]
    [ProducesResponseType(typeof(IEnumerable<InternalDivisionDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetInternalDivisions(int organizationalUnitId, CancellationToken ct)
    {
        var result = await _orgUnitBusiness.GetInternalDivisionsAsync(organizationalUnitId, ct);
        return Ok(result);
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

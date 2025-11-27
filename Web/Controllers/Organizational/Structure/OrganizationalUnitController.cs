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

    /// <summary>
    /// Entpoint para crear unidad organizativa con surcursales
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("organizational-units/create-with-branches")]
    public async Task<IActionResult> CreateWithBranches(
    [FromBody] OrganizationalUnitCreateWithBranchesDtoRequest dto)
    {
        var result = await _orgUnitBusiness.CreateOrganizationalUnitWithBranchesAsync(dto);

        return Ok(new
        {
            success = true,
            message = "Unidad organizacional creada y asignada correctamente.",
            data = result
        });
    }

    [HttpPost("branches/assign")]
    public async Task<IActionResult> AssignBranch([FromBody] OrganizationalUnitBranchDtoRequest dto)
    {
        var result = await _orgUnitBusiness.AddBranchToOrganizationalUnitAsync(dto);

        return Ok(new
        {
            success = true,
            message = "Branch asignada correctamente."
        });
    }

    [HttpDelete("branches/remove")]
    public async Task<IActionResult> RemoveBranch([FromBody] OrganizationalUnitBranchDtoRequest dto)
    {
        var result = await _orgUnitBusiness.RemoveBranchFromOrganizationalUnitAsync(dto);

        return Ok(new
        {
            success = true,
            message = "Branch removida correctamente de la unidad organizacional."
        });
    }

    [HttpGet("{id:int}/detail")]
    public async Task<IActionResult> GetDetail(int id)
    {
        var dto = await _orgUnitBusiness.GetByIdFullAsync(id);

        return Ok(new
        {
            success = true,
            message = "Entidad encontrada",
            data = dto
        });
    }



}

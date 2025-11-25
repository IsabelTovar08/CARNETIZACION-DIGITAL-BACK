using Business.Implementations.Organizational.Structure;
using Business.Interfaces.Organizational.Structure;
using Business.Interfases;
using Data.Implementations.Organizational.Structure;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models.Organizational.Structure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Responses;
using Web.Controllers.Base;

namespace Web.Controllers.Organizational.Structure
{
    public class OrganizationController : GenericController<Organization, OrganizationDtoRequest, OrganizationDto>
    {
        private readonly IOrganizationBusiness _organizationBusiness;
        public OrganizationController(IOrganizationBusiness business, ILogger<OrganizationController> logger) : base(business, logger)
        {
            _organizationBusiness = business;
        }

        [HttpGet("me/organization")]
        [Authorize]
        public async Task<IActionResult> GetMyOrganization()
        {
            var org = await _organizationBusiness.GetMyOrganizationAsync();

            if (org == null)
                return NotFound(ApiResponse<object>.Fail("El usuario no está asociado a ninguna organización."));

            return Ok(ApiResponse<OrganizationDto>.Ok(org));
        }

        [HttpPut("my-organization")]
        [Authorize]
        public async Task<IActionResult> UpdateMyOrganization([FromBody] OrganizationUpdateDtoRequest dto)
        {
            try
            {
                var result = await _organizationBusiness.UpdateMyOrganizationAsync(dto);

                return Ok(ApiResponse<object>.Ok(null, "Organización actualizada correctamente."));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.Fail(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la organización.");
                return StatusCode(500, ApiResponse<object>.Fail("Error interno al actualizar la organización."));
            }
        }

    }
}

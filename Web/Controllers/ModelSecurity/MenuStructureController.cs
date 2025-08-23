using System.Security.Claims;
using System.Threading.Tasks;
using Business.Interfaces.Security;
using Business.Interfases;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models.ModelSecurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace Web.Controllers.ModelSecurity
{
    public class MenuStructureController : GenericController<MenuStructure, MenuStructureRequest, MenuStructureDto>
    {
        protected readonly IMenuStructureBusiness _business;
        public MenuStructureController(IMenuStructureBusiness business, ILogger<MenuStructureController> logger) : base(business, logger)
        {
            _business = business;
        }


        [HttpGet("menu-by-user")]
        public async Task<IActionResult> GetMenuStructureByUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst("sub")?.Value; // fallback para OpenID "sub"

            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(); // # No hay userId válido

            var result = await _business.GetMenuTreeForUserAsync(userId);
            return Ok(result);
        }
    }
}

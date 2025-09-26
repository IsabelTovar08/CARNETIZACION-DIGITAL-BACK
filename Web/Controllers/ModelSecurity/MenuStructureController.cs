using System.Security.Claims;
using Business.Interfaces.Auth;
using Business.Interfaces.Security;
using Entity.DTOs.ModelSecurity.Response;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Security
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MenuStructureDto>>> GetAll()
        {
            var menu = await _menuService.GetMenuAsync();
            return Ok(menu);
        }

        [HttpGet("menu-by-user")]
        public async Task<IActionResult> GetMenuStructureByUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst("sub")?.Value; // fallback para OpenID "sub"

            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(); // # No hay userId válido

            var result = await _menuService.GetMenuForUserAsync(userId);
            return Ok(result);
        }
    }
}

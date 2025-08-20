using Business.Classes;
using Business.Interfaces.Security;
using Business.Services.Auth;
using Business.Services.JWT;
using Data.Implementations.Security;
using Data.Interfaces.Security;
using Entity.DTOs;
using Entity.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.ModelSecurity
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly AuthService _authService;
        private readonly IUserRoleBusiness _userRolBusiness;
        private readonly IMenuStructureBusiness _menuBusiness;

        public AuthController(JwtService jwtService, AuthService userBusiness, IUserRoleBusiness userRolBusiness, IMenuStructureBusiness menuBusiness)
        {
            _jwtService = jwtService;
            _authService = userBusiness;
            _userRolBusiness = userRolBusiness;
            _menuBusiness = menuBusiness;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.LoginAsync(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized("Credenciales inválidas.");
            }

            var roles = await _userRolBusiness.GetRolesByUserIdAsync(user.Id);
            var token = _jwtService.GenerateToken(user.Id.ToString(), user.UserName, roles);
            var menu = await _menuBusiness.GetMenuTreeForUserAsync(user.Id);

            return Ok(new { token , menu});
        }


    }
}

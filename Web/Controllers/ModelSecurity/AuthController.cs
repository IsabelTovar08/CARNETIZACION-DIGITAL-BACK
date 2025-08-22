using Business.Classes;
using Business.Interfaces.Auth;
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
        private readonly IAuthService _authService;

        public AuthController(JwtService jwtService, IAuthService userBusiness, IMenuStructureBusiness menuBusiness)
        {
            _jwtService = jwtService;
            _authService = userBusiness;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.LoginAsync(request);
            if (user == null)
            {
                return Unauthorized("Credenciales inválidas.");
            }

            var token = _jwtService.GenerateToken(user.Id.ToString(), request.Email);

            return Ok(new { token });
        }


    }
}

using Business.Classes;
using Business.Services.Auth;
using Business.Services.JWT;
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
        private readonly UserRoleBusiness _userRolBusiness;

        public AuthController(JwtService jwtService, AuthService userBusiness, UserRoleBusiness userRolBusiness)
        {
            _jwtService = jwtService;
            _authService = userBusiness;
            _userRolBusiness = userRolBusiness;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.LoginAsync(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized("Credenciales inválidas.");
            }

            var roles = await _userRolBusiness.GetRolesByUserId(user.Id);
            var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email, roles);

            return Ok(new { token });
        }


    }
}

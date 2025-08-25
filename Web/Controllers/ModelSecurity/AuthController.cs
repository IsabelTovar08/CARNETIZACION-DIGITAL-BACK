using Business.Classes;
using Business.Interfaces.Auth;
using Business.Interfaces.Security;
using Business.Services.Auth;
using Business.Services.JWT;
using Data.Implementations.Security;
using Data.Interfaces.Security;
using Entity.Context;
using Entity.DTOs;
using Entity.DTOs.Auth;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers.ModelSecurity
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IAuthService _authService;
        private readonly IRefreshTokenService _refreshTokenService;


        public AuthController(IJwtService jwtService, IAuthService userBusiness, IMenuStructureBusiness menuBusiness, IRefreshTokenService refreshTokenService)
        {
            _jwtService = jwtService;
            _authService = userBusiness;
            _refreshTokenService = refreshTokenService;
        }

        public class VerifyCodeRequest
        {
            public int UserId { get; set; }
            public string Code { get; set; }
        }

        /// <summary>
        /// Login:
        /// - Valida credenciales con IAuthService
        /// - Emite Access Token + jti con IJwtService
        /// - Registra y devuelve Refresh Token (hash en BD, valor en claro al cliente)
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // 1) Autenticar usuario (tu lógica actual)
            var user = await _authService.LoginAsync(request);
            if (user == null)
                return Unauthorized("Credenciales inválidas.");

            var verifier = HttpContext.RequestServices.GetRequiredService<UserVerificationService>();
            await verifier.GenerateAndSendAsync(user);

            

            // 2) Emitir Access Token + jti (IMPORTANTE: JwtService debe devolver también jti)
            //var (accessToken, jti) = _jwtService.GenerateToken(user.Id.ToString(), user.UserName);

            // 3) Registrar y devolver Refresh Token (rotación futura)
            //var pair = await _refreshTokenService.IssueAsync(
            //    userId: user.Id,
            //    accessToken: accessToken,
            //    jti: jti,
            //    ip: HttpContext.Connection.RemoteIpAddress?.ToString()
            //);

            // Si quieres, podrías setear el refresh en cookie HttpOnly aquí
            // Response.Cookies.Append("rt", pair.RefreshToken, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict, Expires = DateTimeOffset.UtcNow.AddDays(7) });
            return Ok(new { requieresCode = true, isFirstLogin = !user.Active, userId = user.Id });
            //return Ok(pair);
        }

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeRequest dto)
        {
            var verifier = HttpContext.RequestServices.GetRequiredService<IUserVerificationService>();
            var ok = await verifier.VerifyAsync(dto.UserId, dto.Code);
            if (!ok) return BadRequest(new { message = "Código inválido o expirado." });

            // Emitir Access + Refresh (aquí sí)
            var db = HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
            var user = await db.Set<User>().FirstAsync(u => u.Id == dto.UserId);

            var (accessToken, jti) = _jwtService.GenerateToken(user.Id.ToString(), user.UserName!);
            var pair = await _refreshTokenService.IssueAsync(
                userId: user.Id,
                accessToken: accessToken,
                jti: jti,
                ip: HttpContext.Connection.RemoteIpAddress?.ToString()
            );

            return Ok(pair);
        }

        [HttpPost("resend-code")]
        public async Task<IActionResult> Resend([FromBody] VerifyCodeRequest dto)
        {
            var verifier = HttpContext.RequestServices.GetRequiredService<IUserVerificationService>();
            var ok = await verifier.ResendAsync(dto.UserId);
            if (!ok) return BadRequest(new { message = "Espera antes de reenviar otro código." });
            return NoContent();
        }
        /// <summary>
        /// Refresh:
        /// - Valida el refresh recibido
        /// - Revoca el refresh usado (rotación obligatoria)
        /// - Emite y devuelve nuevo Access Token + nuevo Refresh Token
        /// </summary>
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthTokens>> Refresh([FromBody] RefreshRequest request)
        {
            var pair = await _refreshTokenService.RefreshAsync(
                refreshTokenPlain: request.RefreshToken,
                ip: HttpContext.Connection.RemoteIpAddress?.ToString()
            );

            return Ok(pair);
        }

        /// <summary>
        /// Revoke:
        /// - Revoca un refresh token específico (logout)
        /// </summary>
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RefreshRequest request)
        {
            await _refreshTokenService.RevokeAsync(
                refreshTokenPlain: request.RefreshToken,
                ip: HttpContext.Connection.RemoteIpAddress?.ToString()
            );

            return NoContent();
        }


    }
}

using Business.Interfaces.Auth;
using Business.Interfaces.Security;
using Entity.Context;
using Entity.DTOs.Auth;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Web.Controllers.ModelSecurity
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IAuthService _authService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserVerificationService _verifier;
        private readonly ApplicationDbContext _db;

        public AuthController(
            IJwtService jwtService,
            IAuthService authService,
            IRefreshTokenService refreshTokenService,
            IUserVerificationService verifier,
            ApplicationDbContext db
        )
        {
            _jwtService = jwtService;
            _authService = authService;
            _refreshTokenService = refreshTokenService;
            _verifier = verifier;
            _db = db;
        }

        /// <summary>
        /// 1) Autentica credenciales y envía código de verificación (NO emite JWT aquí).
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.LoginAsync(request);
            if (user is null)
                return Unauthorized(new { message = "Credenciales inválidas." });

            // Opción 1: el servicio NO retorna bool; si falla, lanza excepción.
            await _verifier.GenerateAndSendAsync(user);

            // No emitir JWT aquí
            return Ok(new LoginStep1ResponseDto
            {
                RequiresCode = true,
                IsFirstLogin = !user.Active,
                UserId = user.Id
            });
        }

        /// <summary>
        /// 2) Verifica el código enviado y emite Access/Refresh tokens.
        /// </summary>
        [HttpPost("verify-code")]
        [ProducesResponseType(typeof(AuthTokens), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeRequestDto dto)
        {
            // Verifica el código (expiración, intentos, etc. lo maneja el servicio)
            var ok = await _verifier.VerifyAsync(dto.UserId, dto.Code);
            if (!ok) return BadRequest(new { message = "Código inválido o expirado." });

            // Cargar usuario y emitir tokens
            var user = await _db.Set<User>().FirstOrDefaultAsync(u => u.Id == dto.UserId);
            if (user is null) return BadRequest(new { message = "Usuario no existe." });

            var (accessToken, jti) = _jwtService.GenerateToken(user.Id.ToString(), user.UserName!);

            var pair = await _refreshTokenService.IssueAsync(
                userId: user.Id,
                accessToken: accessToken,
                jti: jti,
                ip: HttpContext.Connection.RemoteIpAddress?.ToString()
            );

            // (Opcional) Cookie HttpOnly para el refresh:
            // Response.Cookies.Append("rt", pair.RefreshToken, new CookieOptions {
            //     HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict,
            //     Expires = DateTimeOffset.UtcNow.AddDays(7)
            // });

            return Ok(pair);
        }

        /// <summary>
        /// Reenvía el código (con cooldown en el servicio).
        /// </summary>
        [HttpPost("resend-code")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.TooManyRequests)]
        public async Task<IActionResult> Resend([FromBody] ResendCodeRequestDto dto)
        {
            var canSend = await _verifier.ResendAsync(dto.UserId);
            if (!canSend)
                return StatusCode((int)HttpStatusCode.TooManyRequests,
                    new { message = "Espera antes de reenviar otro código." });

            return NoContent();
        }

        /// <summary>
        /// Rotación de refresh token (best practice).
        /// </summary>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AuthTokens), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<AuthTokens>> Refresh([FromBody] RefreshRequest request)
        {
            var pair = await _refreshTokenService.RefreshAsync(
                refreshTokenPlain: request.RefreshToken,
                ip: HttpContext.Connection.RemoteIpAddress?.ToString()
            );
            return Ok(pair);
        }

        /// <summary>
        /// Revoca un refresh token (logout).
        /// </summary>
        [HttpPost("revoke")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
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
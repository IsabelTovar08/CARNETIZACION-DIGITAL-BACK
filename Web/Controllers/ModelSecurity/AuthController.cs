using System.Net;
using System.Security.Claims;
using Business.Interfaces.Auth;
using Business.Interfaces.Security;
using Entity.Context;
using Entity.DTOs.Auth;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly ILogger<AuthController> _logger;


        public AuthController(
            IJwtService jwtService,
            IAuthService authService,
            IRefreshTokenService refreshTokenService,
            IUserVerificationService verifier,
            ApplicationDbContext db,
            ILogger<AuthController> logger)
        
        {
            _jwtService = jwtService;
            _authService = authService;
            _refreshTokenService = refreshTokenService;
            _verifier = verifier;
            _db = db;
             _logger = logger;
        }

        /// <summary>
        /// 1) Autentica credenciales y envía código de verificación (NO emite JWT aquí).
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(new LoginStep1ResponseDto { Message = "Solicitud inválida." });

            try
            {
                var user = await _authService.LoginAsync(request);
                if (user is null)
                    return Unauthorized(new LoginStep1ResponseDto { Message = "Credenciales inválidas." });

                await _verifier.GenerateAndSendAsync(user);

                return Ok(new LoginStep1ResponseDto
                {
                    Message = "El código fue enviado exitosamente a tu correo.",
                    isFirtsLogin = user.Active      // ← descomenta si quieres retornarlo al front
                });
            }
            catch (Utilities.Exeptions.ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación en /login para {Email}", request.Email);
                return BadRequest(new LoginStep1ResponseDto { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en /login");
                return StatusCode(500, new LoginStep1ResponseDto
                {
                    Message = "No se pudo enviar el código, revisa los datos ingresados o vuelve a intentarlo."
                });
            }
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
        /// Cambiar la contraseña del usuario.
        /// </summary>
        [HttpPatch("change-password")]
        [Authorize] // ← Requiere usuario autenticado (JWT)
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest dto)
        {
            // Validación básica del payload
            if (dto is null || string.IsNullOrWhiteSpace(dto.NewPassword) || string.IsNullOrWhiteSpace(dto.CurrentPassword))
                return BadRequest("Invalid payload.");

            if (dto.NewPassword != dto.ConfirmNewPassword)
                return BadRequest("NewPassword and ConfirmNewPassword do not match.");

            // Obtener el userId desde los claims del JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid token.");

            // Ejecutar cambio de contraseña
            await _authService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);

            return NoContent(); // 204 sin contenido cuando todo ok
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
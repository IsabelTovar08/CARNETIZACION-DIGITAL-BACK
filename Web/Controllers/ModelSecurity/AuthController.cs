using System.Net;
using System.Security.Claims;
using Business.Interfaces.Auth;
using Business.Interfaces.Security;
using Entity.DTOs.Auth;
using Entity.DTOs.ModelSecurity.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;
using Utilities.Responses;

namespace Web.Controllers.ModelSecurity
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IAuthService _authService;
        private readonly IUserBusiness _userBusiness;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserVerificationService _verifier;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IJwtService jwtService,
            IAuthService authService,
            IRefreshTokenService refreshTokenService,
            IUserVerificationService verifier,
            ILogger<AuthController> logger,
            IUserBusiness userBusiness)
        {
            _jwtService = jwtService;
            _authService = authService;
            _refreshTokenService = refreshTokenService;
            _verifier = verifier;
            _logger = logger;
            _userBusiness = userBusiness;
        }

        /// <summary>
        /// 1) Auth credentials and send verification code (no JWT here).
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginStep1ResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<LoginStep1ResponseDto>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<LoginStep1ResponseDto>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<LoginStep1ResponseDto>), 500)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<LoginStep1ResponseDto>
                {
                    Success = false,
                    Message = "Solicitud inválida.",
                    Data = null
                });
            }

            try
            {
                var user = await _authService.LoginAsync(request);
                if (user is null)
                {
                    return Unauthorized(new ApiResponse<LoginStep1ResponseDto>
                    {
                        Success = false,
                        Message = "Credenciales inválidas.",
                        Data = null
                    });
                }

                await _verifier.GenerateAndSendAsync(user);

                return Ok(new ApiResponse<LoginStep1ResponseDto>
                {
                    Success = true,
                    Message = "El código fue enviado exitosamente a tu correo.",
                    Data = new LoginStep1ResponseDto
                    {
                        isFirtsLogin = user.Active,
                        UserId = user.Id
                    }
                });
            }
            catch (Utilities.Exeptions.ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación en /login para {Email}", request.Email);
                return BadRequest(new ApiResponse<LoginStep1ResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en /login");
                return StatusCode(500, new ApiResponse<LoginStep1ResponseDto>
                {
                    Success = false,
                    Message = "No se pudo enviar el código, revisa los datos ingresados o vuelve a intentarlo.",
                    Data = null
                });
            }
        }

        /// <summary>
        /// 2) Verify code and issue Access/Refresh tokens.
        /// </summary>
        [HttpPost("verify-code")]
        [ProducesResponseType(typeof(AuthTokens), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthTokens>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeRequestDto dto)
        {
            var result = await _verifier.VerifyAsync(dto.UserId, dto.Code);
            if (!result.Success)
                return BadRequest(new ApiResponse<AuthTokens> { Success = false, Message = result.Error });

            // Cargar usuario solo si fue exitoso (1 sola lectura si quieres: puedes mover el GenerateToken al servicio)
            var user = await _userBusiness.GetById(dto.UserId);
            var (accessToken, jti) = _jwtService.GenerateToken(user.Id.ToString(), user.UserName ?? user.EmailPerson);

            AuthTokens pair = await _refreshTokenService.IssueAsync(
                userId: user.Id,
                accessToken: accessToken,
                jti: jti,
                ip: HttpContext.Connection.RemoteIpAddress?.ToString()
            );

            return Ok( pair );
        }


        /// <summary>
        /// Resend verification code (with cooldown).
        /// </summary>
        [HttpPost("resend-code")]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.TooManyRequests)]
        public async Task<IActionResult> Resend([FromBody] ResendCodeRequestDto dto)
        {
            var result = await _verifier.ResendAsync(dto.UserId);
            if (!result.Success)
            {
                return StatusCode((int)HttpStatusCode.TooManyRequests,
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = result.Error,
                        Data = null
                    });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Código reenviado exitosamente",
                Data = null
            });
        }

        /// <summary>
        /// Change password (requires JWT).
        /// </summary>
        [HttpPatch("change-password")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest dto)
        {
            // Validación rápida del payload
            if (dto is null || string.IsNullOrWhiteSpace(dto.NewPassword) || string.IsNullOrWhiteSpace(dto.CurrentPassword))
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Invalid payload.", Data = null });

            if (dto.NewPassword != dto.ConfirmNewPassword)
                return BadRequest(new ApiResponse<object> { Success = false, Message = "NewPassword and ConfirmNewPassword do not match.", Data = null });

            // Obtener userId desde Claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized(new ApiResponse<object> { Success = false, Message = "Invalid token.", Data = null });

            await _authService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);

            // Comentario (ES): Devolvemos 200 con ApiResponse para mantener consistencia
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Password changed successfully.",
                Data = null
            });
        }

        /// <summary>
        /// Refresh token rotation.
        /// </summary>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(ApiResponse<AuthTokens>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthTokens>), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var pair = await _refreshTokenService.RefreshAsync(
                refreshTokenPlain: request.RefreshToken,
                ip: HttpContext.Connection.RemoteIpAddress?.ToString()
            );

            return Ok(pair);
        }

        /// <summary>
        /// Revoke refresh token (logout).
        /// </summary>
        [HttpPost("revoke")]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Revoke([FromBody] RefreshRequest request)
        {
            await _refreshTokenService.RevokeAsync(
                refreshTokenPlain: request.RefreshToken,
                ip: HttpContext.Connection.RemoteIpAddress?.ToString()
            );

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Session revoked.",
                Data = null
            });
        }

        /// <summary>
        /// Request password reset (send email with token/link).
        /// </summary>
        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
        {
            try
            {
                // Comentario (ES): servicio genera token y envía correo
                await _authService.RequestPasswordResetAsync(dto.Email);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Si el correo existe, se envió un enlace para restablecer la contraseña.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en forgot-password");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        /// <summary>
        /// Perform password reset with token.
        /// </summary>
        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
        {
            try
            {
                // Comentario (ES): valida token y actualiza la contraseña (hash)
                var ok = await _authService.ResetPasswordAsync(dto.Email, dto.Token, dto.NewPassword);
                if (!ok)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid or expired token.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Password updated successfully.",
                    Data = null
                });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida en reset-password");
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error en reset-password");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        // DTOs locales (si los mantienes aquí)
        public class ResetPasswordRequestDto
        {
            public string Email { get; set; } = default!;
            public string Token { get; set; } = default!;
            public string NewPassword { get; set; } = default!;
        }

        public class ForgotPasswordRequestDto
        {
            public string Email { get; set; } = default!;
        }
    }
}

using System.Text.Json;
using Business.Interfaces.Auth;
using Business.Interfaces.Notifications;
using Business.Interfaces.Security;
using Business.Services.JWT;
using Data.Interfases.Security;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.DTOs;
using Entity.DTOs.Auth;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Enums.Specifics;
using Entity.Models;
using Infrastructure.Notifications.Interfases;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;
using Utilities.Helpers;
using Utilities.Notifications.Implementations.Templates.Email;
using static Utilities.Helper.EncryptedPassword;

namespace Business.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserService _userService;
        private readonly IUserData _userData;


        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;
        private readonly INotify _notificationSender;
        private readonly INotificationBusiness _notificationBusiness;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserVerificationService _verifier;
        private readonly IDeviceInfoService _deviceInfoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(UserService userService, IConfiguration config, ILogger<AuthService> logger, IUserData userData, INotify notificationSender, INotificationBusiness notificationBusiness, IJwtService jwtService, IRefreshTokenService refreshTokenService, IUserVerificationService verifier,
             IDeviceInfoService deviceInfoService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _userService = userService;
            _config = config;
            _logger = logger;
            _userData = userData;
            _notificationSender = notificationSender;
            _notificationBusiness = notificationBusiness;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _verifier = verifier;
            _deviceInfoService = deviceInfoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> LoginAsync(LoginRequest loginRequest)
        {
            // Validar usuario
            User? user = await _userData.ValidateUserAsync(loginRequest.Email, loginRequest.Password);
            if (user == null)
                throw new ValidationException("Credenciales inválidas");

            // Generar token
            return user;
        }

        public async Task NotifyLogin(string user, int userId)
        {
            await _notificationBusiness.SendTemplateAsync(NotificationTemplateType.Login, user, userId, _deviceInfoService, _httpContextAccessor);

        }

        // Cambiar contraseña validando la contraseña actual
        public async Task ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            // 1) Traer usuario
            var user = await _userData.GetByIdAsync(userId)
                       ?? throw new ValidationException("Usuario no encontrado.");

            if (user.IsDeleted)
                throw new ValidationException("Usuario inactivo.");

            // 2) Verificar contraseña actual
            var valid = VerifyPassword(currentPassword, user.Password);
            if (!valid)
                throw new ValidationException("Credenciales incorrecta"); // mensaje consistente con tu proyecto

            // 3) Reglas mínimas de robustez (opcional)
            if (newPassword.Length < 8)
                throw new ValidationException("La contraseña debe tener al menos 8 caracteres.");

            if (VerifyPassword(user.Password, newPassword))
                throw new ValidationException("La nueva contraseña no puede ser igual a la actual.");

            // 4) Generar hash para la nueva contraseña
            var newHash = EncryptPassword(newPassword);

            // 5) Persistir cambio
            user.Password = newHash;
            //user.UpdatedAt = DateTime.UtcNow;

            await _userData.UpdateAsync(user);

            _logger.LogInformation("Password changed for user {UserId}", userId);
        }


        public async Task<string?> RequestPasswordResetAsync(string email)
        {
            try
            {
                var token = await _userData.RequestPasswordResetAsync(email);

                var resetLink = $"http://localhost:4200/auth/new-password?token={token}&email={email}";

                var model = new Dictionary<string, object>
                {
                    ["title"] = "Recuperar tu acceso",
                    ["recovery_link"] = resetLink,
                    ["expiry_minutes"] = 60,
                    ["button_text"] = "Cambiar Contraseña"
                };

                var html = await EmailTemplates.RenderAsync("ResetPassword.html", model);
                await _notificationSender.NotifyAsync(
                    "email",
                    email,
                    "Restablecer contraseña",
                    html
                );

                if (token == null)
                {
                    _logger.LogWarning("Password reset requested for non-existing email: {Email}", email);
                    return null;
                }

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error requesting password reset for {Email}", email);
                throw new BusinessException("An error occurred while requesting password reset.", ex);
            }
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                var result = await _userData.ResetPasswordAsync(email, token, newPassword);

                if (!result)
                {
                    _logger.LogWarning("Invalid reset attempt for {Email} with token {Token}", email, token);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for {Email}", email);
                throw new BusinessException("An error occurred while resetting the password.", ex);
            }
        }


        /// <summary>
        /// Maneja el flujo completo de login:
        /// - Si el usuario NO tiene 2FA: devuelve tokens inmediatamente.
        /// - Si el usuario SÍ tiene 2FA: envía código y pide verify-code.
        /// </summary>
        public async Task<LoginResultDto> LoginWithTwoFactorFlowAsync(LoginRequest loginRequest, string? ip)
        {
            // 1) Validar usuario y credenciales
            User? user = await _userData.ValidateUserAsync(loginRequest.Email, loginRequest.Password);
            if (user == null)
                throw new ValidationException("Credenciales inválidas");

            // 2) Si el usuario NO tiene 2FA → generar tokens inmediato
            if (user.TwoFactorEnabled == false || user.TwoFactorEnabled == null)
            {
                var (accessToken, jti) = _jwtService.GenerateToken(
                    user.Id.ToString(),
                    user.UserName ?? user.Person.FirstName + " " + user.Person.LastName
                );

                var tokens = await _refreshTokenService.IssueAsync(
                    user.Id,
                    accessToken,
                    jti,
                    ip
                );

                await NotifyLogin(user.Person.FirstName + " " + user.Person.LastName, user.Id);

                return new LoginResultDto
                {
                    RequiresTwoFactor = false,
                    Tokens = tokens,
                    IsFirstLogin = user.Active,
                    UserId = user.Id
                };
            }

            // 3) Si SÍ tiene 2FA → enviar código
            await _verifier.GenerateAndSendAsync(user);

            return new LoginResultDto
            {
                RequiresTwoFactor = true,
                UserId = user.Id,
                IsFirstLogin = user.Active
            };
        }

    }
}
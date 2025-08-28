using System.Text.Json;
using Business.Interfaces.Auth;
using Business.Interfaces.Security;
using Data.Classes.Specifics;
using Data.Interfases.Security;
using Entity.DTOs;
using Entity.DTOs.Auth;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Infrastructure.Notifications.Interfases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;
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


        public AuthService(UserService userService, IConfiguration config, ILogger<AuthService> logger, IUserData userData, INotify notificationSender)
        {
            _userService = userService;
            _config = config;
            _logger = logger;
            _userData = userData;
            _notificationSender = notificationSender;
        }

        public async Task<User> LoginAsync(LoginRequest loginRequest)
        {
            // Validar usuario
            var user = await _userData.ValidateUserAsync(loginRequest.Email, loginRequest.Password);
            if (user == null)
                throw new ValidationException("Credenciales inválidas");


            // Generar token
            return user;
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
                throw new ValidationException("Credenciales inválidas"); // mensaje consistente con tu proyecto

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
    }
}
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Utilities.Exeptions;
using Microsoft.Extensions.Logging;
using Entity.Models;
using Entity.DTOs.Auth;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Response;
using Business.Interfaces.Auth;
using static Utilities.Helper.EncryptedPassword;
using Business.Interfaces.Security;
using Data.Interfases.Security;
using Data.Classes.Specifics;

namespace Business.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserService _userService;
        private readonly IUserData _userData;


        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserService userService, IConfiguration config, ILogger<AuthService> logger, IUserData userData)
        {
            _userService = userService;
            _config = config;
            _logger = logger;
            _userData = userData;
        }

        public async Task<User> LoginAsync(LoginRequest loginRequest)
        {
            // Validar usuario
            var user = await _userService.ValidateUserAsync(loginRequest.Email, loginRequest.Password);
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


    }
}
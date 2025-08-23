using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Utilities.Exeptions;
using Microsoft.Extensions.Logging;
using Entity.Models;
using Entity.DTOs.Auth;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Response;
using Business.Interfaces.Auth;

namespace Business.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserService _userService;


        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserService userService, IConfiguration config, ILogger<AuthService> logger)
        {
            _userService = userService;
            _config = config;
            _logger = logger;
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



    }
}
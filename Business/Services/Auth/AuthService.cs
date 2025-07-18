using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Utilities.Exeptions;
using Microsoft.Extensions.Logging;
using Entity.Models;
using Entity.DTOs.Auth;
using Entity.DTOs;

namespace Business.Services.Auth
{
    public class AuthService
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

        public async Task<User> LoginAsync(string email, string password)
        {
            // Validar usuario
            var user = await _userService.ValidateUserAsync(email, password);
            if (user == null)
                throw new ValidationException("Credenciales inválidas");


            // Generar token
            return user;
        }



    }
}
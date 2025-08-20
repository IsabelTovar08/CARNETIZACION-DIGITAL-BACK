using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Security;
using Business.Services.Auth;
using Data.Classes.Specifics;
using Data.Interfases;
using Data.Interfases.Security;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;
using static Utilities.Helper.EncryptedPassword;

namespace Business.Classes
{
    public class UserBusiness : BaseBusiness<User, UserDtoRequest, UserDTO>, IUserBusiness
    {
        private readonly UserService _userService;
        public readonly IUserData _userData;
        private readonly IUserRoleBusiness _userRolBusiness;

        // Constructor para inyectar dependencias
        public UserBusiness(IUserData userData, ILogger<User> logger, IMapper mapper, UserService userService, IUserRoleBusiness userRolBusiness)
            : base(userData, logger, mapper)
        {
            _userData = userData;
            _userService = userService;
            _userRolBusiness = userRolBusiness;
        }

        // Validación del DTO
        protected void Validate(UserDtoRequest userDTO)
        {
            var errors = new List<string>();

            if (userDTO == null)
            {
                throw new ValidationException("El Usuario no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(userDTO.UserName))
                errors.Add("El Nombre del Usuario es obligatorio.");
            if (string.IsNullOrWhiteSpace(userDTO.Password))
                errors.Add("La contraseña del Usuario es obligatoria.");

            // Si hay errores, lanzar excepción
            if (errors.Any())
            {
                throw new ValidationException(string.Join(" | ", errors));
            }


        }
        /// <summary>
        /// Crea una nueva entidad.
        /// </summary>
        public override async Task<UserDTO> Save(UserDtoRequest userDTO)
        {
            try
            {
                Validate(userDTO);
                var user = _mapper.Map<User>(userDTO);
                user.Password = EncryptPassword(userDTO.Password);

                var created = await _userData.SaveAsync(user);
                _= SendWelcomeEmailAsync(user);
                _= AsignarRol(created.Id);
                return _mapper.Map<UserDTO>(created);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el usuario.");
                throw new ExternalServiceException("Base de datos", "No se pudo crear el usuario.");
            }
        }
        private async Task EnsureEmailIsUnique(string email)
        {
            if (await _userData.FindByEmail(email) != null)
            {
                throw new ValidationException("Ya existe una cuenta asociada a este correo electrónico.");
            }
        }

        private async Task SendWelcomeEmailAsync(User user)
        {
            try
            {
                Console.WriteLine("Inicar a enviar mensaje.");

                await _userService.SendEmailWelcome(user);
                Console.WriteLine("enviadoooooooooo");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar el email de bienvenida.");
            }
        }


        private async Task<bool> AsignarRol(int userId)
        {
            try
            {
                Console.WriteLine("Inicar a asignar rol.");
                UserRoleDtoRequest userRol = new UserRoleDtoRequest()
                {
                    Id = 0,
                    UserId = userId,
                    RolId = 2 // Rol estándar por defecto
                };
                await _userRolBusiness.Save(userRol);
                Console.WriteLine("Rol asignado.");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al asignar el rol por defecto para el nuevo usuario.");
                return false;
            }
        }

    }
}

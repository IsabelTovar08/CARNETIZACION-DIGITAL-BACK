using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Security;
using Business.Services.Auth;
using Data.Classes.Specifics;
using Data.Interfases;
using Data.Interfases.Security;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.DTOs;
using Entity.DTOs.Auth;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Specifics;
using Entity.Models;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;
using Utilities.Helper;
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

        //verifica si el correo ya esta en uso
        private async Task EnsureEmailIsUnique(string email)
        {
            if (await _userData.FindByEmail(email) != null)
            {
                throw new ValidationException("Ya existe una cuenta asociada a este correo electrónico.");
            }
        }

        //envio del correo electronico con un mensaje de bienvenida
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
        public async Task<List<string>> GetUserRolesById(int userId)
        {
           return await _userData.GetUserRolesByIdAsync(userId);
        }


        public async Task<UserMeDto?> GetByIdForMe(int userId, List<string> roles)
        {
            // Define aquí qué roles SON "web" y NO deben traer perfil
            bool isWebRole = roles.Any(r =>
                r.Equals("AdminOrg", StringComparison.OrdinalIgnoreCase) ||
                r.Equals("Backoffice", StringComparison.OrdinalIgnoreCase) ||
                r.Equals("AdminWeb", StringComparison.OrdinalIgnoreCase));

            bool includeProfile = !isWebRole;

            // Trae la entidad con o sin includes, según el rol
            var user = await _userData.GetByIdForMeAsync(userId, true);
            if (user == null) return null;

            // Mapeo completo con AutoMapper
            var dto = _mapper.Map<UserMeDto>(user);

            // Por contrato: si es rol web, CurrentProfile debe ir null
            if (!includeProfile) dto.CurrentProfile = null;

            return dto;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _userData.GetByIdAsync(userId);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario por Id.");
                throw new ExternalServiceException("Base de datos", "No se pudo obtener el usuario.");
            }
        }

        public async Task<UserProfileDto?> GetProfileAsync(int userId)
        {
            try
            {
                var user = await _userData.GetByIdWithPersonAsync(userId);
                if (user == null) return null;

                return _mapper.Map<UserProfileDto>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el perfil del usuario con Id {UserId}", userId);
                throw new ExternalServiceException("Base de datos", "No se pudo obtener el perfil del usuario.");
            }
        }

        public async Task<UserProfileDto?> UpdateProfileAsync(int userId, UserProfileRequestDto dto)
        {
            try
            {
                var user = await _userData.GetByIdWithPersonAsync(userId);
                if (user == null) return null;

                if (user.Person != null)
                {
                    user.Person.FirstName = dto.FirstName;
                    user.Person.LastName = dto.LastName;
                    user.Person.SecondLastName = dto.SecondLastName;
                    user.Person.Phone = dto.Phone;
                    user.Person.Email = dto.Email;
                }

                await _userData.UpdateAsync(user);

                return _mapper.Map<UserProfileDto>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el perfil del usuario {UserId}", userId);
                throw new ExternalServiceException("Base de datos", "No se pudo actualizar el perfil del usuario.");
            }
        }



        /// <summary>
        /// Obtiene todos los usuarios que pertenecen a un rol específico.
        /// </summary>
        public async Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleName)
        {
            IEnumerable<User> users = await _userData.GetUsersByRoleAsync(roleName);

            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }
    }
}

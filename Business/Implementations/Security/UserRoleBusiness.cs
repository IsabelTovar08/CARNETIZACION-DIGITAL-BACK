using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Security;
using Data.Classes.Specifics;
using Data.Interfases;
using Data.Interfases.Security;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business.Classes
{
    public class UserRoleBusiness : BaseBusiness<UserRoles, UserRoleDtoRequest, UserRolDto>, IUserRoleBusiness
    {
        private readonly IUserRoleData _dataUserRol;
        public UserRoleBusiness
            (IUserRoleData dataUserRol, ILogger<UserRoles> logger, IMapper mapper) : base(dataUserRol, logger, mapper)
        {
            _dataUserRol = dataUserRol;
        }

        /// <summary>
        /// Valida que la información del rol de usuario sea correcta antes de procesarla.
        /// </summary>
        /// <param name="userRolDto">DTO con la información del rol de usuario.</param>
        /// <exception cref="ValidationException">
        /// Se lanza cuando el DTO es nulo, o cuando los campos <c>RolId</c> o <c>UserId</c> son nulos.
        /// </exception>
        protected void Validate(UserRoleDtoRequest userRolDto)
        {
            if (userRolDto == null)
                throw new ValidationException("El Rol del usuario no puede ser nulo.");
            if (userRolDto.RolId == null)
                throw new ValidationException("El Rol no puede ser nulo.");
            if (userRolDto.UserId == null)
                throw new ValidationException("El User no puede ser nulo.");
        }

        /// <summary>
        /// Obtiene la lista de roles asignados a un usuario específico.
        /// </summary>
        /// <param name="id">Identificador único del usuario.</param>
        /// <returns>Una lista de nombres de roles asociados al usuario.</returns>
        /// <exception cref="ValidationException">
        /// Se lanza si el ID es nulo o menor o igual a cero.
        /// </exception>
        /// <exception cref="Exception">
        ///   /// Se lanza si ocurre un error inesperado durante la consulta.
        /// </exception>
        public async Task<List<string>> GetRolesByUserIdAsync(int id)
        {
            if (id == null)
                throw new ValidationException("UserRol", $"{id} no puede ser nulo");

            try
            {

                if (id == null || id <= 0)
                    throw new ValidationException("Id", "El ID debe ser mayor que cero");

                var roles = await _dataUserRol.GetRolesByUserId(id);

                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la lista de roles para el usuario con id: {id}");
                throw;
            }
        }


        /// <summary>
        /// Guarda la relación entre un usuario y uno o varios roles.
        /// </summary>
        /// <param name="request">Objeto <see cref="UserRolesRequest"/> con la información a guardar.</param>
        /// <returns>Un valor booleano indicando si la operación fue exitosa.</returns>
        /// <exception cref="ValidationException">
        /// Se lanza si el request es nulo o contiene datos inválidos.
        /// </exception>
        public async Task<bool> SaveUserRolesAsync(UserRolesRequest request)
        {
            if (request == null)
                throw new ValidationException("El request de UserRoles no puede ser nulo.");

            try
            {
                // Aquí podrías agregar validaciones adicionales si es necesario
                var result = await _dataUserRol.SaveUserRoles(request);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la relación Usuario-Rol.");
                throw;
            }
        }
    }
}

using AutoMapper;
using Business.Classes.Base;
using Data.Classes.Specifics;
using Data.Interfases;
using Entity.DTOs;
using Entity.Models;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business.Classes
{
    public class UserRoleBusiness : BaseBusiness<UserRoles, UserRolDto>
    {
        private readonly UserRoleData _dataUserRol;
        public UserRoleBusiness
            (ICrudBase<UserRoles> data, ILogger<UserRoles> logger, IMapper mapper, UserRoleData dataUserRol) : base(data, logger, mapper)
        {
            _dataUserRol = dataUserRol;
        }

        protected void Validate(UserRolDto userRolDto)
        {
            if (userRolDto == null)
                throw new ValidationException("El Rol del usuario no puede ser nulo.");
            if (userRolDto.RolId == null)
                throw new ValidationException("El Rol no puede ser nulo.");
            if (userRolDto.UserId == null)
                throw new ValidationException("El User no puede ser nulo.");
        }


        public async Task<List<string>> GetRolesByUserId(int id)
        {
            if (id == null)
                throw new ValidationException("UserRol", $"{id} no puede ser nulo");

            try
            {

                if (id == null || id <= 0)
                    throw new ValidationException("Id", "El ID debe ser mayor que cero");

                var roles = await _dataUserRol.GetRolesByUserIdAsync(id);

                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la lista de roles para el usuario con id: {id}");
                throw;
            }
        }
    }
}

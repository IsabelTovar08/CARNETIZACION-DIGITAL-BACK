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
    public class RoleBusiness : BaseBusiness<Role, RoleDtoRequest, RolDto>, IRoleBusiness
    {
        public readonly IRoleData _data;
        public RoleBusiness(IRoleData data, ILogger<Role> logger, IMapper mapper) : base(data, logger, mapper)
        {
            _data = data;
        }

        protected void Validate(RoleDtoRequest rol)
        {
            if (rol == null)
                throw new ValidationException("El rol no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(rol.Name))
                throw new ValidationException("El nombre del rol es obligatorio.");

        }


        public async Task<bool> UserIsSuperAdminAsync(List<string> roleIds)
        {
            try
            {
                return await _data.UserIsSuperAdminAsync(roleIds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando si usuario es SuperAdmin");
                throw;
            }
        }

    }
}

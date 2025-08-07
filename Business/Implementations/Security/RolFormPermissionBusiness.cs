using System.Dynamic;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces;
using Data.Interfases;
using Entity.Context;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business.Classes
{
    public class RolFormPermissionBusiness : BaseBusiness<RolFormPermission, RolFormPermissionDtoRequest, RolFormPermissionDto>, IRolFormPermissionBusiness
    {
        private readonly IRolFormPermissionData _rolFormPermissionData;
        public RolFormPermissionBusiness
            (IRolFormPermissionData rolFormPermissionData, ILogger<RolFormPermission> logger, IMapper mapper, ApplicationDbContext context) : base(rolFormPermissionData, logger, mapper)
        {
            _rolFormPermissionData = rolFormPermissionData;
        }

        protected void Validate(RolFormPermissionDtoRequest rolFormPermissionDto)
        {
            if (rolFormPermissionDto == null)
                throw new ValidationException("El permiso por formulario para el rol no puede ser nulo.");

            if (rolFormPermissionDto.RolId == null)
                throw new ValidationException("El Rol es obligatorio.");
            if (rolFormPermissionDto.FormId == null)
                throw new ValidationException("El Formulario es obligatorio.");
            if (rolFormPermissionDto.PermissionId == null)
                throw new ValidationException("El Permiso es obligatorio.");
        }

        public async Task<List<RolFormPermissionsCompletedDto>> GetAllRolFormPermissionsAsync()
        {
            return await _rolFormPermissionData.GetAllRolFormPermissionsAsync();
        }
    }
}

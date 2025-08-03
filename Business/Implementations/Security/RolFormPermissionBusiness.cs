using System.Dynamic;
using AutoMapper;
using Business.Classes.Base;
using Data.Interfases;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business.Classes
{
    public class RolFormPermissionBusiness : BaseBusiness<RolFormPermission, RolFormPermissionDtoRequest, RolFormPermissionDto>
    {
        public RolFormPermissionBusiness
            (ICrudBase<RolFormPermission> data, ILogger<RolFormPermission> logger, IMapper mapper) : base(data, logger, mapper)
        {

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

       
    }
}

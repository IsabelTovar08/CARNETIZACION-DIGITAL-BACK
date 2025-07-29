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
    public class PermissionBusiness : BaseBusiness<Permission, PermissionDtoRequest, PermissionDto>
    {
        public PermissionBusiness(ICrudBase<Permission> data, ILogger<Permission> logger, IMapper mapper) : base(data, logger, mapper)
        {

        }

        protected void Validate(PermissionDtoRequest form)
        {
            if (form == null)
                throw new ValidationException("El permiso no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(form.Name))
                throw new ValidationException("El Nombre del permiso es obligatorio.");

        }
    }
}

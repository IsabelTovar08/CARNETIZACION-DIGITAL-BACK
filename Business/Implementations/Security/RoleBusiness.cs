using AutoMapper;
using Business.Classes.Base;
using Data.Interfases;
using Entity.DTOs;
using Entity.Models;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business.Classes
{
    public class RoleBusiness : BaseBusiness<Role, RolDto>
    {
        public RoleBusiness(ICrudBase<Role> data, ILogger<Role> logger, IMapper mapper) : base(data, logger, mapper)
        {

        }

        protected void Validate(RolDto rol)
        {
            if (rol == null)
                throw new ValidationException("El rol no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(rol.Name))
                throw new ValidationException("El nombre del rol es obligatorio.");

        }

    }
}

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
    public class ModuleBusiness : BaseBusiness<Module, ModuleDtoRequest, ModuleDto>
    {
        public ModuleBusiness
            (ICrudBase<Module> data, ILogger<Module> logger, IMapper mapper) : base(data, logger, mapper)
        {

        }

        protected void Validate(ModuleDtoRequest moduleDto)
        {
            if (moduleDto == null)
                throw new ValidationException("El módulo no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(moduleDto.Name))
                throw new ValidationException("El Nombre del módulo es obligatorio.");
        }


    }
}

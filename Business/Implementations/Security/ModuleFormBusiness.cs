using AutoMapper;
using Business.Classes.Base;
using Data.Classes.Specifics;
using Data.Interfases;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business.Classes
{
    public class ModuleFormBusiness : BaseBusiness<ModuleForm, ModuleFormDtoRequest, ModuleFormDto>
    {
        public ModuleFormBusiness
            (ICrudBase<ModuleForm> data, ILogger<ModuleForm> logger, IMapper mapper) : base(data, logger, mapper)
        {

        }


        protected void Validate(ModuleFormDtoRequest moduleFormDto)
        {
            if (moduleFormDto == null)
                throw new ValidationException("El múdulo del formulario no puede ser nulo.");
            if (moduleFormDto.ModuleId == null)
                throw new ValidationException("El Módulo es obligatorio.");
            if (moduleFormDto.FormId == null)
                throw new ValidationException("El Formulario es obligatorio.");

        }

    }
}

using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Security;
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
    public class ModuleBusiness : BaseBusiness<Module, ModuleDtoRequest, ModuleDto>, IModuleBusiness
    {
        public readonly IModuleData _moduleData;
        public ModuleBusiness
            (ICrudBase<Module> data, ILogger<Module> logger, IMapper mapper, IModuleData moduleData) : base(data, logger, mapper)
        {
            _moduleData = moduleData;
        }

        protected void Validate(ModuleDtoRequest moduleDto)
        {
            if (moduleDto == null)
                throw new ValidationException("El módulo no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(moduleDto.Name))
                throw new ValidationException("El Nombre del módulo es obligatorio.");
        }

        public async Task<List<ModuleDto>> GetModulesWithFormsByAllowedFormsAsync(List<int> allowedFormIds)
        {
            try
            {
                var modules = await _moduleData.GetModulesWithFormsByAllowedFormsAsync(allowedFormIds);
                return _mapper.Map<List<ModuleDto>>(modules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo módulos filtrados por formularios permitidos");
                throw;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;

namespace Business.Interfaces.Security
{
    public interface IModuleBusiness : IBaseBusiness<Module, ModuleDtoRequest, ModuleDto>
    {
        Task<List<ModuleDto>> GetModulesWithFormsByAllowedFormsAsync(List<int> allowedFormIds);
    }
}

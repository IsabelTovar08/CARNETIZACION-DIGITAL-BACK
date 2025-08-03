using Business.Classes;
using Business.Interfases;
using Entity.DTOs;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;
using Web.Controllers.Base;


namespace Web.Controllers.ModelSecurity
{
    public class ModuleController : GenericController<Module,ModuleDtoRequest, ModuleDto>
    {
        public ModuleController(IBaseBusiness<Module, ModuleDtoRequest, ModuleDto> business, ILogger<ModuleController> logger) : base(business, logger)
        {
        }
    }
}

using Business.Classes;
using Business.Interfases;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;
using Web.Controllers.Base;


namespace Web.Controllers.ModelSecurity
{
    public class ModuleController : GenericController<Module, ModuleDto>
    {
        public ModuleController(IBaseBusiness<Module, ModuleDto> business, ILogger<ModuleController> logger) : base(business, logger)
        {
        }
    }
}

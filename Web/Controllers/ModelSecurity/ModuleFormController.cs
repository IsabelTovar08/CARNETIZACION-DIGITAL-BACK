using Business.Classes;
using Business.Interfases;
using Entity.DTOs;
using Entity.DTOs.Create;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;
using Web.Controllers.Base;

namespace Web.Controllers.ModelSecurity
{
    public class ModuleFormController : GenericController<ModuleForm, ModuleFormDto>
    {
        public ModuleFormController(IBaseBusiness<ModuleForm, ModuleFormDto> business, ILogger<ModuleFormController> logger) : base(business, logger)
        {
        }
    }
}

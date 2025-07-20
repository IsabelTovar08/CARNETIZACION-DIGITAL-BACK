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

    public class RolFormPermissionController : GenericController<RolFormPermission, RolFormPermissionDto>
    {
        public RolFormPermissionController(IBaseBusiness<RolFormPermission, RolFormPermissionDto> business, ILogger<RolFormPermissionController> logger)
            : base(business, logger)
        {
        }

    }
    
}

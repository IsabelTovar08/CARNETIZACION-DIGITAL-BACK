using Business.Classes;
using Business.Interfaces;
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

    public class RolFormPermissionController : GenericController<RolFormPermission, RolFormPermissionDtoRequest, RolFormPermissionDto>
    {
        private readonly IRolFormPermissionBusiness _rolFormPermissionBusiness;
        public RolFormPermissionController(IBaseBusiness<RolFormPermission, RolFormPermissionDtoRequest, RolFormPermissionDto> business,
            IRolFormPermissionBusiness rolFormPermissionBusiness,
            ILogger<RolFormPermissionController> logger)
            : base(business, logger)
        {
            _rolFormPermissionBusiness = rolFormPermissionBusiness;
        }

        [HttpGet("permisos-completos")]
        public async Task<IActionResult> GetAllRolFormPermissions()
        {
            var data = await _rolFormPermissionBusiness.GetAllRolFormPermissionsAsync();
            return Ok(data);
        }

    }

}

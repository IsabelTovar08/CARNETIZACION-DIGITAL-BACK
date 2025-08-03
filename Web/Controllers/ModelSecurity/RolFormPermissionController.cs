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

    public class RolFormPermissionController : GenericController<RolFormPermission, RolFormPermissionDtoRequest, RolFormPermissionDto>
    {
        public RolFormPermissionController(IBaseBusiness<RolFormPermission, RolFormPermissionDtoRequest, RolFormPermissionDto> business, ILogger<RolFormPermissionController> logger)
            : base(business, logger)
        {
        }

    }
    
}

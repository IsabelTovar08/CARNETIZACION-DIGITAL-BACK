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
    public class PermissionController : GenericController<Permission, PermissionDtoRequest, PermissionDto>
    {
        public PermissionController(IBaseBusiness<Permission, PermissionDtoRequest, PermissionDto> business, ILogger<Permission> logger) : base(business, logger)
        {
        }
    }
}

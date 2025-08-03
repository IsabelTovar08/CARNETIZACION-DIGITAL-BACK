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
    public class UserRolController : GenericController<UserRoles, UserRoleDtoRequest, UserRolDto>
    {
        public UserRolController(IBaseBusiness<UserRoles, UserRoleDtoRequest, UserRolDto> business, ILogger<UserRolController> logger) : base(business, logger)
        {
        }
    }
}

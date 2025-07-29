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
    public class RolController : GenericController<Role, RoleDtoRequest, RolDto>
    {
        private readonly IBaseBusiness<Role, RoleDtoRequest, RolDto> _business;

        
        public RolController(IBaseBusiness<Role, RoleDtoRequest, RolDto> business, ILogger<RolController> logger)
            : base(business, logger)
        {
            _business = business;
        }
    }
}

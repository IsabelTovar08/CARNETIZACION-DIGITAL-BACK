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
    public class RolController : GenericController<Role, RolDto>
    {
        private readonly IBaseBusiness<Role, RolDto> _business;

        
        public RolController(IBaseBusiness<Role, RolDto> business, ILogger<RolController> logger)
            : base(business, logger)
        {
            _business = business;
        }
    }
}

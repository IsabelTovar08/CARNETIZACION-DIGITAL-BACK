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
    public class UserController : GenericController<User, UserDTO>
    {
        public UserController(IBaseBusiness<User, UserDTO> business, ILogger<UserController> logger) : base(business, logger)
        {
        }
    }
}


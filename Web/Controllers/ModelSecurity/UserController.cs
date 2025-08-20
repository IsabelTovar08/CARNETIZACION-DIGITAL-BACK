using Business.Classes;
using Business.Interfaces.Security;
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
    public class UserController : GenericController<User, UserDtoRequest, UserDTO>
    {
        public UserController(IUserBusiness business, ILogger<UserController> logger) : base(business, logger)
        {
        }
    }
}


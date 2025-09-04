using System.Security.Claims;
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
        protected readonly IUserBusiness _userBusiness;
        public UserController(IUserBusiness business, ILogger<UserController> logger) : base(business, logger)
        {
            _userBusiness = business;
        }



        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var userIdStr = User.FindFirst("id")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(userIdStr) || !int.TryParse(userIdStr, out var userId))
                return Unauthorized(new { message = "Token inválido o sin identificador de usuario." });

            // Roles del JWT
            List<string> roles = await _userBusiness.GetUserRolesById(userId);

            var me = await _userBusiness.GetByIdForMe(userId, roles);
            if (me == null) return NotFound(new { message = "Usuario no encontrado." });

            return Ok(new { status = true, message = "Ok", data = me });
        }
    }
}


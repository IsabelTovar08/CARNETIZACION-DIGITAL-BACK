using Business.Classes;
using Business.Interfaces.Security;
using Business.Interfases;
using Entity.DTOs;
using Entity.DTOs.Auth;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Specifics;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utilities.Exeptions;
using Utilities.Helper;
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

        [HttpPost("verify-password")]
        [Authorize] 
        public async Task<IActionResult> VerifyPassword([FromBody] VerifyPasswordDtoRequest dto )
        {
            //Sacar el userId del token
            var userIdStr = User.FindFirst("id")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdStr))
                return Unauthorized(new { message = "Token inválido" });

            // Busca al usuario en BD
            var userId = int.Parse(userIdStr);

            var user = await _userBusiness.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "Usuario no encontrado." });

            //Verificar contraseña
            bool isValid = EncryptedPassword.VerifyPassword(dto.Password, user.Password!);
            if (!isValid)
                return Ok(new { status = false, message = "Contraseña incorrecta." });

            return Ok(new { status = true, message = "Contraseña verificada correctamente." });
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userIdStr = User.FindFirst("id")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(userIdStr) || !int.TryParse(userIdStr, out var userId))
                return Unauthorized(new { message = "Token inválido o sin identificador de usuario." });

            var dto = await _userBusiness.GetProfileAsync(userId);
            if (dto == null)
                return NotFound(new { success = false, message = "Usuario no encontrado." });

            return Ok(new { success = true, message = "Ok", data = dto });
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileRequestDto dto)
        {
            var userIdStr = User.FindFirst("id")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(userIdStr) || !int.TryParse(userIdStr, out var userId))
                return Unauthorized(new { message = "Token inválido o sin identificador de usuario." });

            var updated = await _userBusiness.UpdateProfileAsync(userId, dto);
            if (updated == null)
                return NotFound(new { success = false, message = "Usuario no encontrado" });

            return Ok(new { success = true, message = "Perfil actualizado correctamente", data = updated });
        }

        [HttpPost("two-factor/toggle")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ToggleTwoFactor()
        {
            // Alternar estado del 2FA solo con el Id
            var result = await _userBusiness.ToggleTwoFactorAsync();

            if (!result)
                return BadRequest("No se pudo alternar el estado del 2FA");

            return Ok(new
            {
                message = "Estado de autenticación en dos pasos actualizado correctamente"
            });
        }


    }
}


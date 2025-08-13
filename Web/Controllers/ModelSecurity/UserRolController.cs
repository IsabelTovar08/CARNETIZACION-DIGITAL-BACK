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
    public class UserRolController : GenericController<UserRoles, UserRoleDtoRequest, UserRolDto>
    {
        protected readonly IUserRoleBusiness _userRoleBusiness;
        public UserRolController(IUserRoleBusiness business, ILogger<UserRolController> logger) : base(business, logger)
        {
            _userRoleBusiness = business;
        }

        /// <summary>
        /// Asigna o actualiza los roles de un usuario en el sistema.
        /// </summary>
        /// <param name="request">Objeto con la información del usuario y los roles a asignar.</param>
        /// <returns>
        /// Devuelve <see cref="IActionResult"/> indicando el resultado de la operación:
        /// - **200 OK** si la asignación fue exitosa.
        /// - **400 BadRequest** si hubo algún error de validación.
        /// - **500 InternalServerError** si ocurrió un error inesperado.
        /// </returns>
        [HttpPost("SaveAll")]
        public async Task<IActionResult> SaveUserRoles([FromBody] UserRolesRequest request)
        {
            try
            {
                // Validación inicial del request
                if (request == null)
                    return BadRequest("La solicitud no puede ser nula.");

                // Llamada a la capa Business para guardar los roles
                var result = await _userRoleBusiness.SaveUserRolesAsync(request);

                // Si el resultado es false, significa que no se guardó nada
                if (!result)
                    return BadRequest("No fue posible asignar los roles al usuario.");

                // Operación exitosa
                return Ok("Roles asignados correctamente.");
            }
            catch (ValidationException vex)
            {
                // Errores de validación controlados
                _logger.LogWarning(vex, "Error de validación en SaveUserRoles");
                return BadRequest(vex.Message);
            }
            catch (Exception ex)
            {
                // Errores inesperados
                _logger.LogError(ex, "Error inesperado en SaveUserRoles");
                return StatusCode(500, "Ocurrió un error interno al asignar los roles.");
            }
        }

    }
}

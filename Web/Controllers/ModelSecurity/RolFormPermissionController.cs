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

    public class RolFormPermissionController : GenericController<RolFormPermission, RolFormPermissionDtoRequest, RolFormPermissionDto>
    {
        private readonly IRolFormPermissionBusiness _rolFormPermissionBusiness;
        public RolFormPermissionController(IBaseBusiness<RolFormPermission, RolFormPermissionDtoRequest, RolFormPermissionDto> business,
            IRolFormPermissionBusiness rolFormPermissionBusiness,
            ILogger<RolFormPermissionController> logger)
            : base(business, logger)
        {
            _rolFormPermissionBusiness = rolFormPermissionBusiness;
        }

        /// <summary>
        /// Obtiene todos los permisos por formulario de roles.
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllRolFormPermissions()
        {
            try
            {
                var result = await _rolFormPermissionBusiness.GetAllRolFormPermissionsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos por formulario de roles.");
                return StatusCode(500, "Error interno en el servidor.");
            }
        }
        /// <summary>
        /// Crea un nuevos permisos por formulario para un rol.
        /// </summary>
        [HttpPost("saveAll")]
        public async Task<IActionResult> CreateAsync([FromBody] RoleFormPermissionsRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("La solicitud no puede ser nula.");

                var result = await _rolFormPermissionBusiness.SaveRoleFormPermissionsAsync(request);
                if (result)
                    return Ok(new { message = "Permisos guardados correctamente." });
                else
                    return StatusCode(500, new { message = "No se pudieron guardar los permisos." });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear permiso por formulario de rol.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el permiso por formulario de rol.");
                return StatusCode(500, "Error interno en el servidor.");
            }
        }

    }

}

using Business.Classes;
using Business.Interfaces.Auth;
using Business.Interfaces.Organizational.Structure;
using Business.Interfaces.Security;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models;
using Entity.Models.Organizational.Structure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utilities.Exeptions;
using Utilities.Responses;
using Web.Controllers.Base;

namespace Web.Controllers.Organizational.Structure
{
    public class ContactOrganizationController : GenericController<ContactOrganization, ContactOrganizationDtoRequest, ContactOrganizationDtoResponse>
    {
        private readonly IContactOrganizationBusiness _contactBusiness;
        private readonly ILogger<ContactOrganizationController> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IUserRoleBusiness _userRoleData;
        private readonly IPersonBusiness _personBusiness;

        public ContactOrganizationController(IContactOrganizationBusiness contactBusiness, ILogger<ContactOrganizationController> logger, ICurrentUser currentUser, IUserRoleBusiness userRoleData, IPersonBusiness personBusiness)
            : base(contactBusiness, logger)
        {
            _contactBusiness = contactBusiness;
            _logger = logger;
            _currentUser = currentUser;
            _userRoleData = userRoleData;
            _personBusiness = personBusiness;
        }

        /// <summary>
        /// Envía una nueva solicitud de contacto organizacional.
        /// </summary>
        [HttpPost("send-request")]
        [AllowAnonymous]
        public async Task<IActionResult> SendRequest([FromBody] ContactOrganizationDtoRequest dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage)
                        ? (e.Exception?.Message ?? "Error de validación.")
                        : e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<object>.Fail("Datos inválidos.", errors));
            }

            try
            {
                var result = await _contactBusiness.CreateContactRequest(dto);
                return Ok(ApiResponse<ContactOrganizationDtoResponse>.Ok(result, "Solicitud enviada correctamente."));
            }
            catch (ValidationException vex)
            {
                _logger.LogWarning(vex, "Validación fallida al enviar solicitud de organización.");
                return BadRequest(ApiResponse<object>.Fail(vex.Message));
            }
            catch (ExternalServiceException esx)
            {
                _logger.LogError(esx, "Error externo al enviar solicitud de organización.");
                return StatusCode(500, ApiResponse<object>.Fail(esx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al enviar solicitud de organización.");
                return StatusCode(500, ApiResponse<object>.Fail("Ocurrió un error interno al procesar la solicitud."));
            }
        }


        /// <summary>
        /// Aprueba o rechaza una solicitud de contacto organizacional.
        /// </summary>
        [HttpPut("approve/{id:int}")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> ApproveRequest(int id, [FromQuery] bool approved)
        {
            try
            {
                //Obtener el usuario autenticado desde el servicio actual
                int userId = _currentUser.UserId;

                if (userId <= 0)
                    return Unauthorized(ApiResponse<object>.Fail("No se pudo determinar el usuario autenticado."));

                //Verificar si el usuario tiene el rol SuperAdmin (ID = 1)
                var roleNames = await _userRoleData.GetRolesByUserIdAsync(userId);
                bool isSuperAdmin = roleNames.Any(r => r.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase));

                if (!isSuperAdmin)
                    return StatusCode(403, ApiResponse<object>.Fail("Acceso denegado. Solo los usuarios con rol SuperAdmin pueden aprobar solicitudes."));

                //Ejecutar la lógica de aprobación o rechazo
                await _contactBusiness.ApproveContactAsync(id, approved);

                //Construir mensaje según acción
                string message = approved
                    ? "La solicitud fue aprobada correctamente."
                    : "La solicitud fue rechazada correctamente.";

                return Ok(ApiResponse<object>.Ok(null, message));
            }
            catch (ValidationException vex)
            {
                _logger.LogWarning(vex, "Validación fallida al aprobar la solicitud de contacto organizacional.");
                return BadRequest(ApiResponse<object>.Fail(vex.Message));
            }
            catch (ExternalServiceException esx)
            {
                _logger.LogError(esx, "Error externo al aprobar solicitud de contacto organizacional.");
                return StatusCode(500, ApiResponse<object>.Fail(esx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al aprobar la solicitud de contacto organizacional.");
                return StatusCode(500, ApiResponse<object>.Fail("Ocurrió un error interno al procesar la solicitud."));
            }
        }
        

        /// <summary>
        /// Obtiene todas las solicitudes pendientes de aprobación.
        /// </summary>
        [HttpGet("pending")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetPendingRequests()
        {
            try
            {
                var list = await _contactBusiness.GetAll();

                if (list == null || !list.Any())
                    return Ok(ApiResponse<IEnumerable<ContactOrganizationDtoResponse>>.Ok(Enumerable.Empty<ContactOrganizationDtoResponse>(), "No hay solicitudes pendientes."));

                return Ok(ApiResponse<IEnumerable<ContactOrganizationDtoResponse>>.Ok(list, "Listado de solicitudes pendientes obtenido correctamente."));
            }
            catch (ExternalServiceException esx)
            {
                _logger.LogError(esx, "Error externo al listar solicitudes pendientes.");
                return StatusCode(500, ApiResponse<object>.Fail(esx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al listar solicitudes pendientes.");
                return StatusCode(500, ApiResponse<object>.Fail("Ocurrió un error interno al obtener las solicitudes."));
            }
        }
    }
}

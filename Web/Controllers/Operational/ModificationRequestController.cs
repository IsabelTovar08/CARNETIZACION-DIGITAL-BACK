using Business.Interfaces.Operational;
using Business.Interfases;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Specifics;
using Entity.Models.Operational;
using Microsoft.AspNetCore.Mvc;
using Utilities.Responses;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    public class ModificationRequestController : GenericController<ModificationRequest, ModificationRequestDto, ModificationRequestResponseDto>
    {
        protected readonly IModificationRequestBusiness _business;
        public ModificationRequestController(IModificationRequestBusiness business, ILogger<ModificationRequestController> logger) : base(business, logger)
        {
            _business = business;
        }

        [HttpGet("my-requests")]
        public async Task<IActionResult> GetMyRequests()
        {
            try
            {
                var list = await _business.GetByCurrentUserAsync();
                return Ok(ApiResponse<IEnumerable<ModificationRequestResponseDto>>.Ok(list, "Solicitudes obtenidas correctamente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<ModificationRequestResponseDto>>.Fail("Error al obtener las solicitudes.", new[] { ex.Message }));
            }
        }


        /// <summary>
        /// Aprueba una solicitud de modificación, actualiza la persona y marca la solicitud como aprobada.
        /// </summary>
        [HttpPost("approve/{id:int}")]
        public async Task<IActionResult> ApproveAsync(int id, [FromBody] MessageDto? message)
        {
            try
            {
                bool result = await _business.ApproveRequestAsync(id, message?.Message);
                return Ok(ApiResponse<bool>.Ok(result, "Solicitud aprobada correctamente."));
            }
            catch (InvalidOperationException ex)
            {
                // Errores esperados de negocio
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                // Errores inesperados
                return StatusCode(500, ApiResponse<bool>.Fail("Ocurrió un error al aprobar la solicitud.", new[] { ex.Message }));
            }
        }

        /// <summary>
        /// Rechaza una solicitud de modificación y actualiza su estado.
        /// </summary>
        [HttpPost("reject/{id:int}")]
        public async Task<IActionResult> RejectAsync(int id, [FromBody] MessageDto? message)
        {
            try
            {
                bool result = await _business.RejectRequestAsync(id, message?.Message);
                return Ok(ApiResponse<bool>.Ok(result, "Solicitud rechazada correctamente."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.Fail("Ocurrió un error al rechazar la solicitud.", new[] { ex.Message }));
            }
        }
    }
}

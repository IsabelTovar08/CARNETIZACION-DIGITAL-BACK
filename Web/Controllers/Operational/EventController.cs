using Business.Interfaces.Operational;
using Business.Interfases;
using Entity.DTOs.Operational;
using Entity.DTOs.Operational.Request;
using Entity.Models.Organizational;
using Microsoft.AspNetCore.Mvc;
using Utilities.Responses;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : GenericController<Event, EventDtoResponse, EventDtoResponse>
    {
        private readonly IEventBusiness _eventBusiness;
        private readonly ILogger<EventController> _logger;

        public EventController(IEventBusiness business, ILogger<EventController> logger)
            : base(business, logger)
        {
            _logger = logger;
            _eventBusiness = business;
        }

        /// <summary>
        /// Crea un evento con accesos, audiencias y genera automáticamente el QR en Base64.
        /// </summary>
        [HttpPost("create-with-access-points")]
        public async Task<IActionResult> CreateWithAccessPoints([FromBody] CreateEventRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _eventBusiness.CreateEventAsync(dto);

            // Recuperar el evento recién creado (con su QR)
            var result = await _eventBusiness.GetEventFullDetailsAsync(id);

            if (result == null)
                return NotFound(new { success = false, message = "Error al obtener el evento recién creado" });

            return CreatedAtAction(nameof(GetById), new { id }, new
            {
                success = true,
                message = "Evento creado correctamente con su código QR generado en Base64",
                data = new
                {
                    eventId = id,
                    eventName = result.Name,
                    eventCode = result.Code,
                    qrCodeBase64 = result.QrCodeBase64
                }
            });
        }

        /// <summary>
        /// Obtiene un evento con accesos y audiencias.
        /// </summary>
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetFullDetails(int id)
        {
            var result = await _eventBusiness.GetEventFullDetailsAsync(id);

            if (result == null)
                return NotFound(new { success = false, message = "Evento no encontrado" });

            return Ok(new
            {
                success = true,
                message = "Detalles completos obtenidos",
                data = result
            });
        }

        /// <summary>
        /// Retorna el número de eventos disponibles.
        /// </summary>
        [HttpGet("available/count")]
        public async Task<ActionResult<ApiResponse<int>>> GetAvailableEventsCount()
        {
            try
            {
                var total = await _eventBusiness.GetAvailableEventsCountAsync();
                var response = ApiResponse<int>.Ok(total, "Eventos disponibles consultados correctamente");
                response.TotalRows = total;
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                var response = ApiResponse<int>.Fail("Validación de negocio fallida", new[] { ex.Message });
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<int>.Fail("Error al consultar eventos disponibles", new[] { ex.Message });
                return BadRequest(response);
            }
        }
    }
}

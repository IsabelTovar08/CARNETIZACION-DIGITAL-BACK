using System.Threading.Tasks;
using Business.Interfaces.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AttendanceController : GenericController<Attendance, AttendanceDtoRequest, AttendanceDtoResponse>
    {
        private readonly IAttendanceBusiness _attendanceBusiness;
        private readonly ILogger<AttendanceController> _logger;

        public AttendanceController(
            IAttendanceBusiness attendanceBusiness,
            ILogger<AttendanceController> logger
        ) : base(attendanceBusiness, logger)
        {
            _attendanceBusiness = attendanceBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Registra asistencia por escaneo (móvil) y retorna la asistencia creada.
        /// Si tu AttendanceBusiness genera el QR, este viajará en el AttendanceDto.
        /// </summary>
        /// <param name="dto">Datos mínimos: PersonId, EventId y AccessPoint (entrada/salida).</param>
        [HttpPost("scan")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAttendance([FromBody] AttendanceDtoRequest dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Solicitud de registro de asistencia sin body.");
                return BadRequest(new { success = false, message = "Solicitud inválida: body vacío." });
            }

            var result = await _attendanceBusiness.RegisterAttendanceAsync(dto);

            if (result == null)
            {
                _logger.LogWarning("No se pudo registrar la asistencia con los datos proporcionados.");
                return BadRequest(new { success = false, message = "No se pudo registrar la asistencia." });
            }

            return Ok(new
            {
                success = true,
                message = "Asistencia registrada correctamente.",
                data = result
            });
        }

        /// <summary>
        /// REGISTRA SOLO LA ENTRADA usando AttendanceDtoRequestSpecific.
        /// Falla si ya existe una entrada abierta para la persona.
        /// </summary>
        [HttpPost("register-entry")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterEntry([FromBody] AttendanceDtoRequestSpecific dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _attendanceBusiness.RegisterEntryAsync(dto);
                return Ok(new
                {
                    success = true,
                    message = "Entrada registrada correctamente.",
                    data = result
                });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al registrar ENTRADA.");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// REGISTRA SOLO LA SALIDA usando AttendanceDtoRequestSpecific.
        /// Falla si no existe una entrada abierta para la persona.
        /// </summary>
        [HttpPost("register-exit")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterExit([FromBody] AttendanceDtoRequestSpecific dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _attendanceBusiness.RegisterExitAsync(dto);
                return Ok(new
                {
                    success = true,
                    message = "Salida registrada correctamente.",
                    data = result
                });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al registrar SALIDA.");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}

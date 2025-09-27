using System;
using System.Threading;
using System.Threading.Tasks;
using Business.Interfaces.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Controllers.Base;
using System.Linq; // necesario para ToList()

namespace Web.Controllers.Operational
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AttendanceController : GenericController<Attendance, AttendanceDtoRequest, AttendanceDtoResponse>
    {
        private readonly IAttendanceBusiness _attendanceBusiness;
        private readonly IAccessPointBusiness _accessPointBusiness;
        private readonly ILogger<AttendanceController> _logger;

        public AttendanceController(
            IAttendanceBusiness attendanceBusiness,
            IAccessPointBusiness accessPointBusiness,
            ILogger<AttendanceController> logger
        ) : base(attendanceBusiness, logger)
        {
            _attendanceBusiness = attendanceBusiness;
            _accessPointBusiness = accessPointBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Registra asistencia por escaneo (móvil) y retorna la asistencia creada.
        /// </summary>
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar ENTRADA.");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// REGISTRA SOLO LA SALIDA usando AttendanceDtoRequestSpecific.
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar SALIDA.");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // NUEVO ENDPOINT: CONSULTA Y FILTRO
        [HttpGet("search")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(
            [FromQuery] int? personId,
            [FromQuery] int? eventId,
            [FromQuery] DateTime? fromUtc,
            [FromQuery] DateTime? toUtc,
            [FromQuery] string? sortBy = "TimeOfEntry",
            [FromQuery] string? sortDir = "DESC",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
        {
            var (items, total) = await _attendanceBusiness.SearchAsync(
                personId, eventId, fromUtc, toUtc, sortBy, sortDir, page, pageSize, ct);

            if (total == 0)
                return Ok(new { items = Array.Empty<object>(), total = 0, page, pageSize, message = "Sin resultados para los filtros aplicados." });

            return Ok(new { items, total, page, pageSize });
        }

        /// <summary>
        /// Registra asistencia a un evento a través de un código QR.
        /// </summary>
        [HttpPost("register-by-qr")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterByQr([FromBody] AttendanceDtoRequest dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.QrCode))
            {
                return BadRequest(new { success = false, message = "Solicitud inválida: debe incluir el QrCode y el PersonId." });
            }

            var result = await _accessPointBusiness.RegisterAttendanceByQrAsync(dto.QrCode, dto.PersonId);

            if (result == null || !result.Success)
            {
                return BadRequest(new { success = false, message = result?.Message ?? "No se pudo registrar la asistencia." });
            }

            return Ok(new
            {
                success = true,
                message = result.Message,
                data = result
            });
        }

        // =========================================================
        // NUEVOS ENDPOINTS: EXPORTACIÓN PDF y EXCEL
        // =========================================================

        /// <summary>
        /// Exporta asistencias a PDF (filtrando opcionalmente por persona, evento y rango de fechas).
        /// </summary>
        [HttpGet("export/pdf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportPdf(
            [FromQuery] int? personId,
            [FromQuery] int? eventId,
            [FromQuery] DateTime? fromUtc,
            [FromQuery] DateTime? toUtc,
            [FromQuery] string? sortBy = "TimeOfEntry",
            [FromQuery] string? sortDir = "DESC",
            CancellationToken ct = default)
        {
            var (items, _) = await _attendanceBusiness.SearchAsync(
                personId, eventId, fromUtc, toUtc, sortBy, sortDir, 1, int.MaxValue, ct);

            var file = await _attendanceBusiness.ExportToPdfAsync(items.ToList(), ct);

            return File(file, "application/pdf", $"Reporte_Asistencias_{DateTime.Now:yyyyMMddHHmm}.pdf");
        }

        /// <summary>
        /// Exporta asistencias a Excel (filtrando opcionalmente por persona, evento y rango de fechas).
        /// </summary>
        [HttpGet("export/excel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportExcel(
            [FromQuery] int? personId,
            [FromQuery] int? eventId,
            [FromQuery] DateTime? fromUtc,
            [FromQuery] DateTime? toUtc,
            [FromQuery] string? sortBy = "TimeOfEntry",
            [FromQuery] string? sortDir = "DESC",
            CancellationToken ct = default)
        {
            var (items, _) = await _attendanceBusiness.SearchAsync(
                personId, eventId, fromUtc, toUtc, sortBy, sortDir, 1, int.MaxValue, ct);

            var file = await _attendanceBusiness.ExportToExcelAsync(items.ToList(), ct);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Reporte_Asistencias_{DateTime.Now:yyyyMMddHHmm}.xlsx");
        }
    }
}

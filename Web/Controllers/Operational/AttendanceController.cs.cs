using Business.Interfaces.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Reports;
using Entity.Models.Organizational;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Controllers.Base;

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

        // ---------------- EXISTENTES ----------------

        [HttpPost("scan")]
        public async Task<IActionResult> RegisterAttendance([FromBody] AttendanceDtoRequest dto)
        {
            if (dto == null)
                return BadRequest(new { success = false, message = "Solicitud inválida: body vacío." });

            var result = await _attendanceBusiness.RegisterAttendanceAsync(dto);
            if (result == null)
                return BadRequest(new { success = false, message = "No se pudo registrar la asistencia." });

            return Ok(new { success = true, message = "Asistencia registrada correctamente.", data = result });
        }

        [HttpPost("register-entry")]
        public async Task<IActionResult> RegisterEntry([FromBody] AttendanceDtoRequestSpecific dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _attendanceBusiness.RegisterEntryAsync(dto);
                return Ok(new { success = true, message = "Entrada registrada correctamente.", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar ENTRADA.");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("register-exit")]
        public async Task<IActionResult> RegisterExit([FromBody] AttendanceDtoRequestSpecific dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _attendanceBusiness.RegisterExitAsync(dto);
                return Ok(new { success = true, message = "Salida registrada correctamente.", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar SALIDA.");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("search")]
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
                return Ok(new { items = Array.Empty<object>(), total = 0, page, pageSize, message = "Sin resultados." });

            return Ok(new { items, total, page, pageSize });
        }

        [HttpPost("register-by-qr")]
        public async Task<IActionResult> RegisterByQr([FromBody] AttendanceDtoRequest dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.QrCode))
                return BadRequest(new { success = false, message = "Debe incluir el QrCode y el PersonId." });

            var result = await _accessPointBusiness.RegisterAttendanceByQrAsync(dto.QrCode, dto.PersonId);

            if (result == null || !result.Success)
                return BadRequest(new { success = false, message = result?.Message ?? "No se pudo registrar la asistencia." });

            return Ok(new { success = true, message = result.Message, data = result });
        }
    }
}

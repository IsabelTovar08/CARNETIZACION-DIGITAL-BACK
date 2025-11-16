using Business.Interfaces.Auth;
using Business.Interfaces.Operational;
using Data.Interfases.Security;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly ILogger<AttendanceController> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IPersonData _personData;

        public AttendanceController(
            IAttendanceBusiness attendanceBusiness,
            IPersonData personData,
            ICurrentUser currentUser,
            ILogger<AttendanceController> logger
        ) : base(attendanceBusiness, logger)
        {
            _attendanceBusiness = attendanceBusiness;
            _personData = personData;
            _currentUser = currentUser;
            _logger = logger;
        }

        // =========================================================
        // ✅ REGISTRO AUTOMÁTICO DE ASISTENCIA POR QR DEL EVENTO
        // =========================================================
        //[HttpPost("register-by-qr")]
        //public async Task<IActionResult> RegisterByQr([FromBody] AttendanceDtoRequest dto)
        //{
        //    if (dto == null || dto.PersonId <= 0)
        //        return BadRequest(new { success = false, message = "Debe incluir el QrCode válido y el PersonId." });

        //    try
        //    {
        //        var result = await _attendanceBusiness.RegisterAttendanceByQrAsync(dto.QrCode, dto.PersonId);

        //        if (result == null || !result.Success)
        //            return BadRequest(new { success = false, message = result?.Message ?? "No se pudo registrar la asistencia." });

        //        return Ok(new
        //        {
        //            success = true,
        //            message = result.Message,
        //            data = result
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error al registrar asistencia mediante QR.");
        //        return StatusCode(StatusCodes.Status500InternalServerError, new
        //        {
        //            success = false,
        //            message = "Error interno al registrar asistencia mediante QR.",
        //            error = ex.Message
        //        });
        //    }
        //}

        // =========================================================
        // REGISTRO GENERAL DE ASISTENCIA (MÓVIL O MANUAL)
        // =========================================================
        [HttpPost("scan")]
        public async Task<IActionResult> RegisterAttendance([FromBody] AttendanceDtoRequest dto)
        {
            if (dto == null)
                return BadRequest(new { success = false, message = "Solicitud inválida: body vacío." });

            var result = await _attendanceBusiness.RegisterAttendanceAsync(dto);
            if (result == null)
                return BadRequest(new { success = false, message = "No se pudo registrar la asistencia." });

            return Ok(new { success = true, message = result.Message, data = result });
        }

        // =========================================================
        // ✅ REGISTRO MANUAL DE ENTRADA (con token → persona asociada)
        // =========================================================
        [HttpPost("register-entry")]
        public async Task<IActionResult> RegisterEntry([FromBody] AttendanceDtoRequestSpecific dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                AttendanceDtoResponse result = await _attendanceBusiness.RegisterEntryAsync(dto);

                return Ok(new
                {
                    success = result.Success,
                    message = result.Message,
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar ENTRADA.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Error interno al registrar la entrada.",
                    error = ex.Message
                });
            }
        }

        // REGISTRO DE SALIDA 
        [HttpPost("register-exit")]
        public async Task<IActionResult> RegisterExit([FromBody] AttendanceDtoRequestSpecific dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // 5️⃣ Registrar la salida
                var result = await _attendanceBusiness.RegisterExitAsync(dto);

                return Ok(new
                {
                    success = result.Success,
                    message = result.Message,
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar SALIDA.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Error interno al registrar la salida.",
                    error = ex.Message
                });
            }
        }

        // =========================================================
        // BÚSQUEDA Y EXPORTACIÓN DE ASISTENCIAS
        // =========================================================
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

        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportPdf(
            [FromQuery] int? personId,
            [FromQuery] int? eventId,
            [FromQuery] DateTime? fromUtc,
            [FromQuery] DateTime? toUtc,
            CancellationToken ct = default)
        {
            var (items, _) = await _attendanceBusiness.SearchAsync(
                personId, eventId, fromUtc, toUtc, "TimeOfEntry", "DESC", 1, int.MaxValue, ct);

            var file = await _attendanceBusiness.ExportToPdfAsync(items, ct);
            return File(file, "application/pdf", $"Reporte_Asistencias_{DateTime.Now:yyyyMMddHHmm}.pdf");
        }

        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportExcel(
            [FromQuery] int? personId,
            [FromQuery] int? eventId,
            [FromQuery] DateTime? fromUtc,
            [FromQuery] DateTime? toUtc,
            CancellationToken ct = default)
        {
            var (items, _) = await _attendanceBusiness.SearchAsync(
                personId, eventId, fromUtc, toUtc, "TimeOfEntry", "DESC", 1, int.MaxValue, ct);

            var file = await _attendanceBusiness.ExportToExcelAsync(items, ct);
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Reporte_Asistencias_{DateTime.Now:yyyyMMddHHmm}.xlsx");
        }
    }
}

using Business.Interfaces.Operational;
using Entity.DTOs.Operational;
using Entity.Models.Organizational;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : GenericController<Attendance, AttendanceDto, AttendanceDto>
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
        /// Endpoint para registrar asistencia por QR.
        /// </summary>
        [HttpPost("scan")]
        public async Task<IActionResult> RegisterAttendance([FromBody] AttendanceDto dto)
        {
            var result = await _attendanceBusiness.RegisterAttendanceAsync(dto);

            if (result == null)
                return BadRequest(new { success = false, message = "No se pudo registrar la asistencia." });

            return Ok(new { success = true, message = "Asistencia registrada correctamente.", data = result });
        }
    }
}

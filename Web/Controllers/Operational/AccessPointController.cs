using Business.Interfaces.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace Web.Controllers.Operational
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessPointController : GenericController<AccessPoint, AccessPointDtoRequest, AccessPointDtoResponsee>
    {
        private readonly IAccessPointBusiness _business;
        private readonly ILogger<AccessPointController> _logger;

        public AccessPointController(IAccessPointBusiness business, ILogger<AccessPointController> logger)
            : base(business, logger)
        {
            _business = business;
            _logger = logger;
        }

        /// <summary>
        /// Registra un punto de acceso y genera su código QR.
        /// </summary>
        /// <param name="dto">Datos del punto de acceso</param>
        /// <returns>AccessPoint con su QR generado</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccessPointDtoRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _business.RegisterAsync(dto);
                if (created == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "No se pudo registrar el punto de acceso. Verifique los datos."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Punto de acceso registrado correctamente.",
                    data = created
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar AccessPoint");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno al registrar el punto de acceso."
                });
            }
        }
    }
}

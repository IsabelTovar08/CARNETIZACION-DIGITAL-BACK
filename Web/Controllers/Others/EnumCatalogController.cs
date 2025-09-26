using Business.Interfaces.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Others
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnumCatalogController : ControllerBase
    {
        private readonly IEnumCatalogService _service;

        public EnumCatalogController(IEnumCatalogService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna un catálogo de Enums según el tipo de enum solicitado.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string type)
        {
            try
            {
                var result = await _service.GetByTypeAsync(type);
                return Ok(result);
            }
            catch
            {
                return BadRequest($"El tipo '{type}' no es válido.");
            }
        }
    }
}

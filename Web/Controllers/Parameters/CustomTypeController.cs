using Business.Interfaces.Parameters;
using Business.Interfases;
using Entity.DTOs.Parameter.Request;
using Entity.DTOs.Parameter.Response;
using Entity.Models.Parameter;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace Web.Controllers.Parameters
{
    public class CustomTypeController : GenericController<CustomType, CustomTypeRequest, CustomTypeDto>
    {
        private readonly ICustomTypeBusiness _customTypeBusiness;
        public CustomTypeController(ICustomTypeBusiness customTypeBusiness, ILogger<CustomTypeController> logger) : base(customTypeBusiness, logger)
        {
            _customTypeBusiness = customTypeBusiness;
        }

        [HttpGet("get-by-name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("El nombre de la categoría es obligatorio.");

            var result = await _customTypeBusiness.GetTypesByCategoryNameAsync(name);

            if (!result.Any())
                return NotFound("No se encontraron tipos para la categoría especificada.");

            return Ok(result);
        }
    }
}

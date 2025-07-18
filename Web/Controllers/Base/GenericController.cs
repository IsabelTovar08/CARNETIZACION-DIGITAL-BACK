using Business.Interfases;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;

namespace Web.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class GenericController<TEntity, TDto> : ControllerBase
    {
        protected readonly IBaseBusiness<TEntity, TDto> _business;
        protected readonly ILogger _logger;

        public GenericController(IBaseBusiness<TEntity, TDto> business, ILogger logger)
        {
            _business = business;
            _logger = logger;
        }

        public GenericController()
        {
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            try
            {
                var entities = await _business.GetAll();
                return Ok(entities);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener datos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            try
            {
                var entity = await _business.GetById(id);
                return Ok(entity);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el ID: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Entidad no encontrada con ID: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener entidad con ID: {Id}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var created = await _business.Save(dto);
                return CreatedAtAction(nameof(GetById), new { id = ((dynamic)created).Id }, created);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear entidad");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear entidad");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("update")]
        public virtual async Task<IActionResult> Update([FromBody] TDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var updated = await _business.Update(dto);
                return Ok(updated);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar entidad");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Entidad no encontrada al actualizar");
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar entidad");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _business.Delete(id);
                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar entidad");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Entidad no encontrada al eliminar");
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar entidad");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }

}

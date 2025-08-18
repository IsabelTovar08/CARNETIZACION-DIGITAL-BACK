using Business.Interfases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;

namespace Web.Controllers.Base
{
    /// <summary>
    /// Controlador genérico que implementa operaciones CRUD y manejo de estado lógico
    /// para cualquier entidad que implemente la capa de negocio <see cref="IBaseBusiness{T, DRequest, D}"/>.
    /// </summary>
    /// <typeparam name="T">Entidad principal</typeparam>
    /// <typeparam name="DRequest">DTO de entrada para crear o actualizar</typeparam>
    /// <typeparam name="D">DTO de salida o de respuesta</typeparam>
    
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class GenericController<T, DRequest, D> : ControllerBase
    {
        protected readonly IBaseBusiness<T, DRequest, D> _business;
        protected readonly ILogger _logger;

        /// <summary>
        /// Constructor del controlador genérico.
        /// </summary>
        /// <param name="business">Instancia de la capa de negocio que gestiona la entidad.</param>
        /// <param name="logger">Instancia para registro de logs.</param>
        public GenericController(IBaseBusiness<T, DRequest, D> business, ILogger logger)
        {
            _business = business;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las entidades.
        /// </summary>
        /// <returns>Lista de entidades.</returns>
        //[Authorize]
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

        /// <summary>
        /// Obtiene una entidad específica por su ID.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>Entidad encontrada o error si no existe.</returns>
        //[Authorize]
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

        /// <summary>
        /// Crea una nueva entidad.
        /// </summary>
        /// <param name="dto">Datos de la entidad a crear.</param>
        /// <returns>Entidad creada con su nuevo ID.</returns>
        //[Authorize]
        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] DRequest dto)
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

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="dto">Datos actualizados de la entidad.</param>
        /// <returns>Entidad actualizada.</returns>
        //[Authorize]
        [HttpPut("update")]
        public virtual async Task<IActionResult> Update([FromBody] DRequest dto)
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

        /// <summary>
        /// Elimina una entidad por su ID.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>Código 200 si se elimina correctamente.</returns>
        //[Authorize]
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

        /// <summary>
        /// Alterna el estado lógico (activo/inactivo) de una entidad.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>Mensaje de confirmación.</returns>
        //[Authorize]
        [HttpPatch("{id}/toggle-active")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El id debe ser mayor que cero.");
            }

            try
            {
                var result = await _business.ToggleActiveAsync(id);
                return Ok(new { Message = $"Estado lógico actualizado correctamente para Id {id}." });
            }
            catch (ValidationException vex)
            {
                _logger.LogWarning(vex, $"Validación fallida para ToggleActive con Id: {id}");
                return BadRequest(vex.Message);
            }
            catch (ExternalServiceException esex)
            {
                _logger.LogError(esex, $"Error externo al actualizar estado lógico para Id: {id}");
                return StatusCode(500, esex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al actualizar estado lógico para Id: {id}");
                return StatusCode(500, "Error interno en el servidor.");
            }
        }
    }
}

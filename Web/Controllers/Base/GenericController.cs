using Business.Interfases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;
using Utilities.Responses;

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
                var entities = await _business.GetAll(); // IEnumerable<D>
                // Si tu capa Business puede brindar el total, colócalo aquí.
                return Ok(ApiResponse<IEnumerable<D>>.Ok(entities, "Listado obtenido"));
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener datos");
                return StatusCode(500, ApiResponse<object>.Fail(ex.Message));
            }
        }

        /// <summary>
        /// Obtiene todas las entidades activas.
        /// </summary>
        /// <returns>Lista de entidades activas filtrando IsDeleted : False.</returns>
        //[Authorize]
        [HttpGet("Active")]
        public virtual async Task<IActionResult> GetActive()
        {
            try
            {
                var entities = await _business.GetActive(); // IEnumerable<D>
                return Ok(ApiResponse<IEnumerable<D>>.Ok(entities, "Listado activo obtenido"));
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener datos");
                return StatusCode(500, ApiResponse<object>.Fail(ex.Message));
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
                var entity = await _business.GetById(id); // D
                return Ok(ApiResponse<D>.Ok(entity, "Entidad encontrada"));
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el ID: {Id}", id);
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Entidad no encontrada con ID: {Id}", id);
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener entidad con ID: {Id}", id);
                return StatusCode(500, ApiResponse<object>.Fail(ex.Message));
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
                var created = await _business.Save(dto); // D
                // Intento obtener el Id de la respuesta sin forzar contrato
                var id = TryGetId(created);
                var response = ApiResponse<D>.Ok(created, "Entidad creada");

                if (id is not null)
                    return CreatedAtAction(nameof(GetById), new { id }, response);

                // Si no hay Id, devolvemos 201 sin location
                return StatusCode(201, response);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear entidad");
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear entidad");
                return StatusCode(500, ApiResponse<object>.Fail(ex.Message));
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
                var updated = await _business.Update(dto); // D
                return Ok(ApiResponse<D>.Ok(updated, "Entidad actualizada"));
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar entidad");
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Entidad no encontrada al actualizar");
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar entidad");
                return StatusCode(500, ApiResponse<object>.Fail(ex.Message));
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
                return Ok(ApiResponse<object>.Ok(null, "Eliminado correctamente"));
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar entidad");
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Entidad no encontrada al eliminar");
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar entidad");
                return StatusCode(500, ApiResponse<object>.Fail(ex.Message));
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
                return BadRequest(ApiResponse<object>.Fail("El id debe ser mayor que cero."));
            }

            try
            {
                await _business.ToggleActiveAsync(id);
                return Ok(ApiResponse<object>.Ok(null, $"Estado lógico actualizado correctamente para Id {id}."));
            }
            catch (ValidationException vex)
            {
                _logger.LogWarning(vex, $"Validación fallida para ToggleActive con Id: {id}");
                return BadRequest(ApiResponse<object>.Fail(vex.Message));
            }
            catch (ExternalServiceException esex)
            {
                _logger.LogError(esex, $"Error externo al actualizar estado lógico para Id: {id}");
                return StatusCode(500, ApiResponse<object>.Fail(esex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al actualizar estado lógico para Id: {id}");
                return StatusCode(500, ApiResponse<object>.Fail("Error interno en el servidor."));
            }
        }

        // ==========================
        // Helpers privados
        // ==========================

        // Intenta leer una propiedad "Id" (por convención) sin forzar interfaz
        private static int? TryGetId(object? entity)
        {
            if (entity is null) return null;
            var prop = entity.GetType().GetProperty("Id");
            if (prop is null) return null;
            var value = prop.GetValue(entity);
            if (value is int i) return i;
            if (value is long l) return (int)l;
            if (int.TryParse(value?.ToString(), out var parsed)) return parsed;
            return null;
        }
    }
}

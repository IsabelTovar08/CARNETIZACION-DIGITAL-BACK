using Business.Interfaces.Logging;
using Entity.DTOs.Specifics;
using Microsoft.AspNetCore.Mvc;
using Utilities.Responses;

namespace Web.Controllers.Logging
{
    /// <summary>
    /// Controlador API para consultar lotes de importación y sus filas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ImportBatchController : ControllerBase
    {
        private readonly IImportHistoryBusiness _business;

        /// <summary>
        /// Inicializa una nueva instancia del controlador.
        /// </summary>
        public ImportBatchController(IImportHistoryBusiness business)
        {
            _business = business;
        }

        /// <summary>Obtiene todos los lotes de importación.</summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ImportBatchDto>>>> GetAll()
        {
            var result = await _business.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<ImportBatchDto>>.Ok(result, "Lotes obtenidos correctamente."));
        }

        /// <summary>Obtiene un lote específico por Id.</summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<ImportBatchDto>>> GetById(int id)
        {
            var result = await _business.GetByIdAsync(id);
            if (result == null)
                return NotFound(ApiResponse<ImportBatchDto>.Fail($"No se encontró el lote con Id={id}"));

            return Ok(ApiResponse<ImportBatchDto>.Ok(result, "Lote obtenido correctamente."));
        }

        /// <summary>Obtiene todas las filas de un lote.</summary>
        [HttpGet("{id:int}/rows")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ImportBatchRowTableDto>>>> GetRows(int id)
        {
            var result = await _business.GetRowsAsync(id);
            return Ok(ApiResponse<IEnumerable<ImportBatchRowDto>>.Ok(
                result,
                "Filas obtenidas correctamente."
            ));
        }

        /// <summary>Obtiene solo las filas con error de un lote.</summary>
        [HttpGet("{id:int}/rows/errors")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ImportBatchRowDetailDto>>>> GetErrorRows(int id)
        {
            var result = await _business.GetErrorRowsAsync(id);
            return Ok(ApiResponse<IEnumerable<ImportBatchRowDetailDto>>.Ok(
                result,
                "Filas con error obtenidas correctamente."
            ));
        }
    }
}

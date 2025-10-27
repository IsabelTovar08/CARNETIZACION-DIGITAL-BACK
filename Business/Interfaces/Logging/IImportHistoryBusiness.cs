using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Specifics;

namespace Business.Interfaces.Logging
{
    /// <summary>
    /// Servicio de negocio para registrar auditorías de importación por lotes/filas.
    /// </summary>
    public interface IImportHistoryBusiness
    {
        Task<int> StartBatchAsync(ImportBatchStartDto dto);
        Task AppendRowAsync(ImportBatchRowDto rowDto);
        Task CompleteBatchAsync(ImportBatchCompleteDto dto);

        /// <summary>Obtiene todos los lotes de importación.</summary>
        Task<IEnumerable<ImportBatchDto>> GetAllAsync();

        /// <summary>Obtiene un lote específico por Id.</summary>
        Task<ImportBatchDto?> GetByIdAsync(int id);

        /// <summary>Obtiene todas las filas de un lote.</summary>
        Task<IEnumerable<ImportBatchRowDto>> GetRowsAsync(int batchId);

        /// <summary>Obtiene solo las filas con error de un lote.</summary>
        Task<IEnumerable<ImportBatchRowDetailDto>> GetErrorRowsAsync(int batchId);
    }
}

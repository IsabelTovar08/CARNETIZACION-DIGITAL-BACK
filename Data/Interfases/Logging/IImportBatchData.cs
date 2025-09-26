using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Specifics;
using Entity.Models.Operational.BulkLoading;

namespace Data.Interfases.Logging
{
    public interface IImportBatchData : ICrudBase<ImportBatch>
    {
        Task CompleteAsync(int batchId, int successCount, int errorCount);

        /// <summary>Obtiene todos los lotes de importación.</summary>
        Task<IEnumerable<ImportBatch>> GetAllAsync();

        /// <summary>Obtiene un lote específico por Id.</summary>
        Task<ImportBatch?> GetByIdAsync(int id);

        /// <summary>Obtiene todas las filas de un lote.</summary>
        Task<IEnumerable<ImportBatchRow>> GetRowsAsync(int batchId);

        /// <summary>Obtiene solo las filas con error de un lote.</summary>
        Task<IEnumerable<ImportBatchRow>> GetErrorRowsAsync(int batchId);
    }
}

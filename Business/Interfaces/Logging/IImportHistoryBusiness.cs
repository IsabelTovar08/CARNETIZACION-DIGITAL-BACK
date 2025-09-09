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
    }
}

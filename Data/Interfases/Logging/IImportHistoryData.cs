using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Specifics;

namespace Data.Interfases.Logging
{
    public interface IImportHistoryData
    {
        Task<int> CreateBatchAsync(string source, string? fileName, string? startedBy, int totalRows, string? contextJson);
        Task AddOrUpdateRowAsync(int batchId, ImportBatchRowLog row);
        Task CompleteBatchAsync(int batchId, int successCount, int errorCount);
    }
}

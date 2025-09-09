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
    }
}

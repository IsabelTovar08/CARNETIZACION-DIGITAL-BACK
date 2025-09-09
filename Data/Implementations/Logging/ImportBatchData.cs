using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.Classes.Base;
using Data.Interfases.Logging;
using Entity.Context;
using Entity.DTOs.Specifics;
using Entity.Models.Operational.BulkLoading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Logging
{
    public class ImportBatchData : BaseData<ImportBatch>, IImportBatchData
    {
        public ImportBatchData(ApplicationDbContext context, ILogger<ImportBatch> logger)
            : base(context, logger) { }


        public async Task CompleteAsync(int batchId, int successCount, int errorCount)
        {
            var batch = await _context.Set<ImportBatch>().FirstOrDefaultAsync(b => b.Id == batchId);
            if (batch == null)
                throw new InvalidOperationException("No se encontró el lote de importación.");

            batch.SuccessCount = successCount;
            batch.ErrorCount = errorCount;
            batch.EndedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}

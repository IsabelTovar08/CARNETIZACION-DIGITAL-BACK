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


        /// <inheritdoc/>
        public async Task<IEnumerable<ImportBatch>> GetAllAsync()
        {
            return await _context.Set<ImportBatch>()
                .AsNoTracking()
                .OrderByDescending(b => b.StartedAt)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ImportBatch?> GetByIdAsync(int id)
        {
            return await _context.Set<ImportBatch>()
                .AsNoTracking()
                .Include(b => b.Rows)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ImportBatchRow>> GetRowsAsync(int batchId)
        {
            return await _context.ImportBatchRows
              .Include(r => r.PersonDivisionProfile)
                  .ThenInclude(pdp => pdp.Person)
              .Include(r => r.PersonDivisionProfile)
                  .ThenInclude(pdp => pdp.InternalDivision)
                      .ThenInclude(div => div.OrganizationalUnit)
              .Include(r => r.Card)
                  .ThenInclude(c => c.Status)
              .Where(r => r.ImportBatchId == batchId)
              .ToListAsync();

        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ImportBatchRow>> GetErrorRowsAsync(int batchId)
        {
            return await _context.Set<ImportBatchRow>()
                .AsNoTracking()
                .Where(r => r.ImportBatchId == batchId && !r.Success)
                .OrderBy(r => r.RowNumber)
                .ToListAsync();
        }
    }
}

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
            var batches = await _context.ImportBatches
            .AsNoTracking()
            .OrderByDescending(b => b.StartedAt)
            .Select(b => new ImportBatch
            {
                Id = b.Id,
                StartedAt = b.StartedAt,
                StartedBy = b.StartedBy,
                TotalRows = b.TotalRows,
                EndedAt = b.EndedAt,
                SuccessCount = b.SuccessCount,
                ErrorCount = b.ErrorCount,
                ContextJson = b.ContextJson,
                FileName = b.FileName,

                // Trae el usuario directamente sin FK ni navegación
                StartedByUser = _context.Users
                    .Include(u => u.Person)
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Id == b.StartedBy)
            })
            .ToListAsync();
            return batches;

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
              .Include(r => r.IssuedCard)
                  .ThenInclude(pdp => pdp.Person)
              .Include(r => r.IssuedCard)
                  .ThenInclude(pdp => pdp.InternalDivision)
                      .ThenInclude(div => div.OrganizationalUnit)
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

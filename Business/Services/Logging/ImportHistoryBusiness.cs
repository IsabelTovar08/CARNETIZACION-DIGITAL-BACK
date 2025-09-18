using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces.Logging;
using Data.Interfases.Logging;
using Entity.DTOs.Specifics;
using Entity.Models.Operational.BulkLoading;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Logging
{
    /// <summary>
    /// Servicio de negocio para persistir historial de importaciones al realizar carga masiva a través de excel.
    /// </summary>
    public class ImportHistoryBusiness : IImportHistoryBusiness
    {
        private readonly IImportBatchData _batchData;
        private readonly IImportBatchRowData _rowData;
        private readonly IMapper _mapper;

        public ImportHistoryBusiness(IImportBatchData batchData, IImportBatchRowData rowData, IMapper mapper)
        {
            _batchData = batchData;
            _rowData = rowData;
            _mapper = mapper;
        }

        public async Task<int> StartBatchAsync(ImportBatchStartDto dto)
        {
            var entity = _mapper.Map<ImportBatch>(dto);
            var saved = await _batchData.SaveAsync(entity);
            return saved.Id;
        }

        public async Task AppendRowAsync(ImportBatchRowDto rowDto)
        {
            var entity = _mapper.Map<ImportBatchRow>(rowDto);
            await _rowData.SaveAsync(entity);
        }

        public Task CompleteBatchAsync(ImportBatchCompleteDto dto)
        {
           return _batchData.CompleteAsync(dto.ImportBatchId, dto.SuccessCount, dto.ErrorCount);
        }


        /// <inheritdoc/>
        public async Task<IEnumerable<ImportBatchDto>> GetAllAsync()
        {
            var batches = await _batchData.GetAllAsync();
            return _mapper.Map<IEnumerable<ImportBatchDto>>(batches);
        }

        /// <inheritdoc/>
        public async Task<ImportBatchDto?> GetByIdAsync(int id)
        {
            var batch = await _batchData.GetByIdAsync(id);
            return _mapper.Map<ImportBatchDto?>(batch);
        }

        /// <inheritdoc/>
        /// <inheritdoc/>
        public async Task<IEnumerable<ImportBatchRowTableDto>> GetRowsAsync(int batchId)
        {
            var rows = await _batchData.GetRowsAsync(batchId);
            return _mapper.Map<IEnumerable<ImportBatchRowTableDto>>(rows);
        }


        /// <inheritdoc/>
        public async Task<IEnumerable<ImportBatchRowDetailDto>> GetErrorRowsAsync(int batchId)
        {
            var rows = await _batchData.GetErrorRowsAsync(batchId);
            return _mapper.Map<IEnumerable<ImportBatchRowDetailDto>>(rows);
        }
    }
}

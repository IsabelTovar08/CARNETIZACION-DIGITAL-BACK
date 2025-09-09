using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Logging;
using Entity.Context;
using Entity.Models.Operational.BulkLoading;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Logging
{
    public class ImportBatchRowData : BaseData<ImportBatchRow>, IImportBatchRowData
    {
        public ImportBatchRowData(ApplicationDbContext context, ILogger<ImportBatchRow> logger)
            : base(context, logger) { }

    }
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Interfaces.Services
{
    public interface IExportService
    {
        Task<byte[]> ExportToPdfAsync<T>(IEnumerable<T> data, string title, CancellationToken cancellationToken);
        Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, string sheetName, CancellationToken cancellationToken);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfases.Security
{
    public interface IBackupData
    {
        Task<BackupResultDto> BackupNowAsync(string db, string dir, bool copyOnly, bool verify, CancellationToken ct);
        Task RestoreNowAsync(string db, string backupFilePath, bool forceCloseConnections, CancellationToken ct);
    }
}

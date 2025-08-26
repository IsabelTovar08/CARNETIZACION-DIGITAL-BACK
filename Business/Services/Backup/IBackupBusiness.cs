using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Backup
{
    public interface IBackupBusiness
    {
        Task<BackupResultDto> CreateBackupAsync(BackupRequestDto request, CancellationToken ct = default);
        Task RestoreBackupAsync(RestoreRequestDto request, CancellationToken ct = default);
    }
}

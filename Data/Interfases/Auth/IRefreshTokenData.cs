using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Auth;

namespace Data.Interfases.Auth
{
    public interface IRefreshTokenData : ICrudBase<RefreshToken>
    {
        Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);

    }
}

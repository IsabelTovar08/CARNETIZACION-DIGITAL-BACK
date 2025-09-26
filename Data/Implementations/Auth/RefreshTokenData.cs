using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases;
using Data.Interfases.Auth;
using Entity.Context;
using Entity.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Auth
{
    public class RefreshTokenData : BaseData<RefreshToken>, IRefreshTokenData
    {
        public RefreshTokenData(ApplicationDbContext context, ILogger<RefreshToken> logger) : base(context, logger)
        {
        }

        public Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken ct = default)
        {
            return _context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.TokenHash == tokenHash, ct);
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        {

            return _context.SaveChangesAsync(ct);
        }
    }
}

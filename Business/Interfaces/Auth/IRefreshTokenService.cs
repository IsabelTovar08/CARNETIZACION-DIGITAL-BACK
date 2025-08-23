using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Auth;
using Entity.Models.Auth;

namespace Business.Interfaces.Auth
{
    public interface IRefreshTokenService
    {
        Task<AuthTokens> IssueAsync(int userId, string accessToken, string jti, string? ip = null);
        Task<AuthTokens> RefreshAsync(string refreshTokenPlain, string? ip = null);
        Task RevokeAsync(string refreshTokenPlain, string? ip = null);
    }
}

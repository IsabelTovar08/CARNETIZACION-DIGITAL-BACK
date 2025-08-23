using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Entity.Context;
using Entity.DTOs.Auth;
using Entity.Models.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services.JWT
{
    public class RefreshTokenService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public RefreshTokenService(ApplicationDbContext db, IConfiguration config)
        {
            _db = db; _config = config;
        }

        // 👉 Genera y persiste un refresh ligado al jti del access generado
        public async Task<AuthTokens> IssueAsync(int userId, string accessToken, string jti)
        {
            var (plain, hash) = GenerateRefreshToken();
            var days = int.Parse(_config["JwtSettings:RefreshTokenDays"]!);
            var now = DateTime.UtcNow;

            var rt = new RefreshToken
            {
                TokenHash = hash,
                JwtId = jti,
                UserId = userId,
                Created = now,
                Expires = now.AddDays(days)
            };

            _db.RefreshTokens.Add(rt);
            await _db.SaveChangesAsync();

            return new AuthTokens { AccessToken = accessToken, RefreshToken = plain };
        }

        // 👉 Valida y rota: revoca el actual y emite nuevo access + refresh
        public async Task<AuthTokens> RefreshAsync(string refreshTokenPlain, Func<(int userId, string username, List<string> roles)> loadUserAndRolesByRefresh, Func<string, string, List<string>, (string token, string jti)> issueAccess)
        {
            var hash = Hash(refreshTokenPlain);

            var rt = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == hash);
            if (rt == null || rt.IsExpired || rt.IsRevoked)
                throw new SecurityTokenException("Invalid refresh token.");

            // Carga el usuario + roles desde tu capa (evita acoplar EF aquí si no quieres)
            var (userId, username, roles) = loadUserAndRolesByRefresh();

            // Revoca el token usado
            rt.Revoked = DateTime.UtcNow;

            // Emite nuevo access con NUEVO jti
            var (newAccess, newJti) = issueAccess(userId.ToString(), username, roles);

            // Crea nuevo refresh
            var (newPlain, newHash) = GenerateRefreshToken();
            var days = int.Parse(_config["JwtSettings:RefreshTokenDays"]!);
            var newRt = new RefreshToken
            {
                TokenHash = newHash,
                JwtId = newJti,
                UserId = userId,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(days)
            };
            rt.ReplacedByTokenHash = newHash;

            _db.RefreshTokens.Add(newRt);
            await _db.SaveChangesAsync();

            return new AuthTokens { AccessToken = newAccess, RefreshToken = newPlain };
        }

        public async Task RevokeAsync(string refreshTokenPlain)
        {
            var hash = Hash(refreshTokenPlain);
            var rt = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == hash);
            if (rt == null || rt.IsRevoked) return;

            rt.Revoked = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        // ---- helpers ----
        private static (string plain, string hash) GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);        // 512 bits
            var plain = Convert.ToBase64String(bytes);
            var hash = Hash(plain);
            return (plain, hash);
        }

        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }
    }
}

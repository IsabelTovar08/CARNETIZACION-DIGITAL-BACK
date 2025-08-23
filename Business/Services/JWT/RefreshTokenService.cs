using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Auth;
using Business.Interfaces.Security;
using Data.Interfases.Auth;
using Entity.DTOs.Auth;
using Entity.Models.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services.JWT
{
    /// <summary>
    /// Servicio de negocio responsable de:
    /// - Emitir refresh tokens al momento del login (IssueAsync)
    /// - Refrescar/rotar el par de tokens (RefreshAsync): revoca el refresh usado, emite nuevo access y nuevo refresh
    /// - Revocar refresh tokens (RevokeAsync), útil para logout
    /// 
    /// Buenas prácticas:
    /// - Nunca se guarda el refresh token en claro; se almacena su HASH (SHA-256)
    /// - Rotación obligatoria: cada uso del refresh invalida el anterior y genera uno nuevo
    /// - Asociación con el Access Token mediante JwtId (jti) para trazabilidad/vinculación
    /// </summary>
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenData _data; 
        private readonly IConfiguration _config;
        private readonly IJwtService _jwtService;
        private readonly IUserBusiness _userBusiness;

        public RefreshTokenService(
            IRefreshTokenData data,
            IConfiguration config,
            IJwtService jwtService,
            IUserBusiness userBusiness)
        {
            _data = data;
            _config = config;
            _jwtService = jwtService;
            _userBusiness = userBusiness;
        }

        /// <summary>
        /// Emite un refresh token nuevo para un access token ya generado.
        /// - Genera un valor aleatorio (plain) y su hash
        /// - Persiste el hash junto con UserId, JwtId, fechas de creación y expiración
        /// - Devuelve el par { AccessToken, RefreshTokenPlain } al cliente
        /// </summary>
        public async Task<AuthTokens> IssueAsync(int userId, string accessToken, string jti, string? ip = null)
        {
            // 1) Generar valor aleatorio (en claro) + hash
            var (plain, hash) = GenerateRefreshToken();

            // 2) Calcular expiración según configuración (días)
            var days = int.Parse(_config["JwtSettings:RefreshTokenDays"]!);
            var now = DateTime.UtcNow;

            // 3) Construir entidad de refresh (se guarda SOLO el hash)
            var rt = new RefreshToken
            {
                TokenHash = hash,
                JwtId = jti,          // Vincula este refresh con el access token emitido (jti)
                UserId = userId,
                Created = now,
                Expires = now.AddDays(days)
                // Opcional: CreatedByIp = ip (si agregas el campo)
            };

            // 4) Persistir en BD (hash y metadatos)
            await _data.SaveAsync(rt);

            // 5) Retornar al cliente: access existente + refresh en claro
            return new AuthTokens { AccessToken = accessToken, RefreshToken = plain };
        }

        /// <summary>
        /// Refresca/rota tokens:
        /// - Valida el refresh recibido (por hash): existe, no expirado, no revocado
        /// - Revoca el refresh actual (rotación obligatoria)
        /// - Emite un NUEVO access token (nuevo jti) y un NUEVO refresh token
        /// - Persiste el nuevo refresh y enlaza ReplacedByTokenHash
        /// - Devuelve el nuevo par { AccessToken, RefreshToken }
        /// </summary>
        public async Task<AuthTokens> RefreshAsync(string refreshTokenPlain, string? ip = null)
        {
            // 1) Hashear el valor recibido (nunca comparamos en claro)
            var hash = Hash(refreshTokenPlain);

            // 2) Buscar el refresh correspondiente en BD
            var rt = await _data.GetByHashAsync(hash);
            if (rt == null || rt.IsExpired || rt.IsRevoked)
                throw new SecurityTokenException("Invalid refresh token.");

            // 3) Cargar el usuario dueño del refresh (para emitir el nuevo access)
            var user = await _userBusiness.GetById(rt.UserId)
                       ?? throw new SecurityTokenException("User not found.");

            // 4) Rotación: marcar el refresh actual como revocado (ya no podrá reutilizarse)
            rt.Revoked = DateTime.UtcNow;
            // Opcional: rt.RevokedByIp = ip; (si agregas el campo en el modelo)

            // 5) Emitir un nuevo Access Token con NUEVO jti (mitiga replay)
            var (newAccess, newJti) = _jwtService.GenerateToken(user.Id.ToString(), user.UserName);

            // 6) Generar un nuevo refresh token (valor en claro + hash) y persistirlo
            var (newPlain, newHash) = GenerateRefreshToken();
            var days = int.Parse(_config["JwtSettings:RefreshTokenDays"]!);

            var newRt = new RefreshToken
            {
                TokenHash = newHash,
                JwtId = newJti,                   // Se asocia al nuevo access token emitido
                UserId = user.Id,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(days)
                // Opcional: CreatedByIp = ip;
            };

            // 7) Traza de rotación: enlaza el nuevo hash en el token anterior
            rt.ReplacedByTokenHash = newHash;

            // 8) Persistir nuevo refresh; si tu repositorio usa el mismo DbContext y rastrea 'rt',
            //    este SaveAsync típicamente guardará también la revocación de 'rt'.
            await _data.SaveAsync(newRt);

            // 9) Retornar el nuevo par al cliente
            return new AuthTokens { AccessToken = newAccess, RefreshToken = newPlain };
        }

        /// <summary>
        /// Revoca un refresh token:
        /// - Si existe y no está revocado, lo marca como revocado y persiste el cambio.
        /// - Útil para logout o invalidación manual.
        /// </summary>
        public async Task RevokeAsync(string refreshTokenPlain, string? ip = null)
        {
            // 1) Hashear el valor recibido
            var hash = Hash(refreshTokenPlain);

            // 2) Buscar el refresh correspondiente
            var rt = await _data.GetByHashAsync(hash);
            if (rt == null || rt.IsRevoked) return;

            // 3) Marcar como revocado y persistir
            rt.Revoked = DateTime.UtcNow;
            // Opcional: rt.RevokedByIp = ip;
            await _data.SaveChangesAsync();
        }

        // ----------------- Helpers privados -----------------

        /// <summary>
        /// Genera un refresh token seguro:
        /// - 64 bytes aleatorios (512 bits) → Base64 (valor en claro para el cliente)
        /// - HASH SHA-256 del valor (lo que se persiste en BD)
        /// </summary>
        private static (string plain, string hash) GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64); // 512 bits de entropía
            var plain = Convert.ToBase64String(bytes);      // valor que verá el cliente
            var hash = Hash(plain);                         // lo que se guarda en BD
            return (plain, hash);
        }

        /// <summary>
        /// Calcula el hash SHA-256 (hex) del input.
        /// Nota: Nunca guardes el refresh en claro en BD.
        /// </summary>
        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }
    }
}

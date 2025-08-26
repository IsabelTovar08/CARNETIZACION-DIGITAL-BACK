using Business.Interfaces.Auth;
using Entity.Context;
using Entity.Models;
using Infrastructure.Notifications.Interfases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Utilities.Notifications.Implementations.Templates.Email;

namespace Business.Services.Auth
{
    // Este servicio sirve como intermediario para la
    // generación, envío y verificación de los códigos creados temporalmente
    public class UserVerificationService : IUserVerificationService
    {
        // Inyección de dependencias
        private readonly ApplicationDbContext _db;
        private readonly INotify _notify;
        private readonly ICodeGenerator _gen;
        private readonly ICodeHasher _hasher;
        private readonly IClock _clock;
        private readonly IConfiguration _cfg;

        public UserVerificationService(
            ApplicationDbContext db, INotify notify, ICodeGenerator gen,
            ICodeHasher hasher, IClock clock, IConfiguration cfg)
        {
            _db = db;
            _notify = notify;
            _gen = gen;
            _hasher = hasher;
            _clock = clock;
            _cfg = cfg;
        }

        private static string GetVerifiedEmail(User user)
        {
            // 1) Prioriza email real
            string? toRaw = user.Person?.Email ?? user.UserName;

            if (string.IsNullOrWhiteSpace(toRaw))
                throw new InvalidOperationException("El usuario no tiene email configurado.");

            // 2) Normaliza/valida (admite "Nombre <email@dom.com>")
            MailAddress addr = new MailAddress(toRaw.Trim());
            return addr.Address; // solo "email@dom.com"
        }

        public async Task GenerateAndSendAsync(User user)
        {
            // Para asegurar que se trae el email de la persona
            User? loaded = await _db.Set<User>()
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Id == user.Id && !u.IsDeleted);

            user = loaded ?? throw new InvalidOperationException("Usuario no encontrado.");

            int digits = int.Parse(_cfg["Codes:Digits"] ?? "5");
            int ttl = int.Parse(_cfg["Codes:TtlMinutes"] ?? "10");

            string code = _gen.Generate(digits);

            DateTimeOffset now = _clock.UtcNow;
            user.TempCodeHash = _hasher.Hash(code);
            user.TempCodeAttempts = 0;
            user.TempCodeConsumedAt = null;
            user.TempCodeCreatedAt = now;
            user.TempCodeExpiresAt = now.AddMinutes(ttl);

            await _db.SaveChangesAsync();

            Dictionary<string, object> model = new Dictionary<string, object>
            {
                ["title"] = "Código de Verificación",
                ["verification_code"] = code,
                ["expiry_minutes"] = ttl,
            };

            string html = await EmailTemplates.RenderByKeyAsync("verify", model);

            // Validar/normalizar destinatario
            string to = GetVerifiedEmail(user);

            await _notify.NotifyAsync("email", to, "Verifica tu identidad", html);
        }

        public async Task<bool> VerifyAsync(int userId, string code)
        {
            int maxAttempts = int.Parse(_cfg["Codes:MaxAttempts"] ?? "5");
            DateTimeOffset now = _clock.UtcNow;

            User? user = await _db.Set<User>()
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

            if (user is null) return false;

            if (string.IsNullOrEmpty(user.TempCodeHash)) return false;
            if (user.TempCodeConsumedAt.HasValue) return false;
            if (!user.TempCodeExpiresAt.HasValue || now > user.TempCodeExpiresAt.Value) return false;
            if (user.TempCodeAttempts >= maxAttempts) return false;

            user.TempCodeAttempts++;

            bool ok = _hasher.Verify(code, user.TempCodeHash);
            if (!ok)
            {
                await _db.SaveChangesAsync();
                return false;
            }

            user.TempCodeConsumedAt = now;
            if (!user.Active) user.Active = true;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResendAsync(int userId)
        {
            int cooldownSeconds = int.Parse(_cfg["Codes:ResendCooldownSeconds"] ?? "50");
            DateTimeOffset now = _clock.UtcNow;

            User? user = await _db.Set<User>()
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

            if (user is null) return false;

            if (user.TempCodeCreatedAt.HasValue &&
                (now - user.TempCodeCreatedAt.Value).TotalSeconds < cooldownSeconds)
                return false;

            await GenerateAndSendAsync(user);
            return true;
        }
    }
}

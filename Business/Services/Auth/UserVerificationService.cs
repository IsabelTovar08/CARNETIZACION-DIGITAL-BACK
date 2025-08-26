using Business.Interfaces.Auth;
using Entity.Context;
using Entity.Models;
using Infrastructure.Notifications.Interfases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Notifications.Implementations.Templates.Email;

namespace Business.Services.Auth

//Este servicio sirve como intermediario para la
//generación, envío y verificación de los códigos creados temporalmente
{
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

        public async Task GenerateAndSendAsync(User user)
        {
            var digits = int.Parse(_cfg["Codes:Digits"] ?? "6");
            var ttl = int.Parse(_cfg["Codes:TtlMinutes"] ?? "10");

            var code = _gen.Generate(digits);

            var now = _clock.UtcNow;
            user.TempCodeHash = _hasher.Hash(code);
            user.TempCodeAttempts = 0;
            user.TempCodeConsumedAt = null;
            user.TempCodeCreatedAt = now;
            user.TempCodeExpiresAt = now.AddMinutes(ttl);

            await _db.SaveChangesAsync();

           

            
                var model = new Dictionary<string, object> 
                { 
                 ["title"] = "Código de Verificación", 
                 ["verification_code"] = code, 
                 ["expiry_minutes"] = ttl, //
                }; 
                var html = await EmailTemplates.RenderByKeyAsync("verify", model);
                
                await _notify.NotifyAsync( "email", user.UserName ?? user.Person.Email, "Verifica tu identidad", html );
        }

        public async Task<bool> VerifyAsync(int userId, string code)
        {
            var maxAttempts = int.Parse(_cfg["Codes:MaxAttempts"] ?? "5");
            var now = _clock.UtcNow;

            var user = await _db.Set<User>()
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
            if (user is null) return false;

            if (string.IsNullOrEmpty(user.TempCodeHash)) return false;
            if (user.TempCodeConsumedAt.HasValue) return false;
            if (!user.TempCodeExpiresAt.HasValue || now > user.TempCodeExpiresAt.Value) return false;
            if (user.TempCodeAttempts >= maxAttempts) return false;

            user.TempCodeAttempts++;

            var ok = _hasher.Verify(code, user.TempCodeHash);
            if (!ok) { await _db.SaveChangesAsync(); return false; }

            user.TempCodeConsumedAt = now;
            if (!user.Active) user.Active = true;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResendAsync(int userId)
        {
            var cooldownSeconds = int.Parse(_cfg["Codes:ResendCooldownSeconds"] ?? "50");
            var now = _clock.UtcNow;

            var user = await _db.Set<User>()
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

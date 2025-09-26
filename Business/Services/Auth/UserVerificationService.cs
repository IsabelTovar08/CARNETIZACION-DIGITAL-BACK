using Business.Interfaces.Auth;
using Data.Interfases.Security;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.Context;
using Entity.DTOs.Auth;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Infrastructure.Notifications.Interfases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Utilities.Notifications.Implementations.Templates.Email;

namespace Business.Services.Auth
{
    // Este servicio sirve como intermediario para la
    // generación, envío y verificación de los códigos creados temporalmente
    public class UserVerificationService : IUserVerificationService
    {
        // Inyección de dependencias
        private readonly INotify _notify;
        private readonly ICodeGenerator _gen;
        private readonly ICodeHasher _hasher;
        private readonly IClock _clock;
        private readonly IConfiguration _cfg;
        private readonly IUserData _userData;

        public UserVerificationService(INotify notify, ICodeGenerator gen,
            ICodeHasher hasher, IClock clock, IConfiguration cfg,
            IUserData userData)
        {
            _notify = notify;
            _gen = gen;
            _hasher = hasher;
            _clock = clock;
            _cfg = cfg;
            _userData = userData;
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
            await GenerateAndSendAsync(user, true); // por defecto, resetea
        }

        public async Task GenerateAndSendAsync(User user, bool resetResendAttempts)
        {
            User? loaded = await _userData.GetByIdAsync(user.Id);

            user = loaded ?? throw new InvalidOperationException("Usuario no encontrado.");

            int digits = int.Parse(_cfg["Codes:Digits"] ?? "5");
            int ttl = int.Parse(_cfg["Codes:TtlMinutes"] ?? "10");

            string code = _gen.Generate(digits);

            DateTimeOffset now = _clock.UtcNow;
            user.TempCodeHash = _hasher.Hash(code);
            user.TempCodeAttempts = 0;          // intentos de verificación del código
            user.TempCodeConsumedAt = null;
            user.TempCodeCreatedAt = now;       // marca el último envío/generación
            user.TempCodeExpiresAt = now.AddMinutes(ttl);

            if (resetResendAttempts)
            {
                user.TempCodeAttempts = 0;           // ← reset SOLO de reenvíos
                user.TempCodeResendBlockedUntil = null; // ← quita bloqueo
            }

            await _userData.UpdateAsync(user);

            var model = new Dictionary<string, object>
            {
                ["title"] = "Código de Verificación",
                ["verification_code"] = code,
                ["expiry_minutes"] = ttl,
            };

            string html = await EmailTemplates.RenderByKeyAsync("verify", model);
            string to = GetVerifiedEmail(user);
            await _notify.NotifyAsync("email", to, "Verifica tu identidad", html);
        }

        public async Task<VerifyResult> VerifyAsync(int userId, string code)
        {
            int maxAttempts = int.Parse(_cfg["Codes:MaxAttempts"] ?? "5");

            var tz = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time")
                : TimeZoneInfo.FindSystemTimeZoneById("America/Bogota");

            var now = TimeZoneInfo.ConvertTime(_clock.UtcNow, tz);

            var user = await _userData.GetByIdAsync(userId);
            if (user is null) return VerifyResult.Fail("Usuario no existe.");

            if (string.IsNullOrEmpty(user.TempCodeHash)) return VerifyResult.Fail("No hay código generado.");
            if (user.TempCodeConsumedAt.HasValue) return VerifyResult.Fail("El código ya fue usado.");
            if (!user.TempCodeExpiresAt.HasValue || now > user.TempCodeExpiresAt.Value)
                return VerifyResult.Fail("El código expiró.");
            if (user.TempCodeAttempts >= maxAttempts) return VerifyResult.Fail("Máximos intentos alcanzados.");

            user.TempCodeAttempts++;

            if (!_hasher.Verify(code, user.TempCodeHash))
            {
                await _userData.UpdateAsync(user);
                return VerifyResult.Fail("Código incorrecto.");
            }

            // Éxito
            user.TempCodeConsumedAt = now;
            if (!user.Active) user.Active = true;
            user.TempCodeAttempts = 0;
            user.TempCodeResendBlockedUntil = null;

            await _userData.UpdateAsync(user);
            return VerifyResult.Ok();
        }




        public async Task<VerifyResult> ResendAsync(int userId)
        {
            int cooldownSeconds = int.Parse(_cfg["Codes:ResendCooldownSeconds"] ?? "60");
            int maxResends = int.Parse(_cfg["Codes:MaxResendAttempts"] ?? "5");
            int blockMinutes = 60;

            var tz = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time")
                : TimeZoneInfo.FindSystemTimeZoneById("America/Bogota");

            var now = TimeZoneInfo.ConvertTime(_clock.UtcNow, tz);

            var user = await _userData.GetByIdAsync(userId);
            if (user is null) return VerifyResult.Fail("Usuario no existe.");

            if (user.TempCodeResendBlockedUntil.HasValue && now < user.TempCodeResendBlockedUntil.Value)
                return VerifyResult.Fail("Usuario bloqueado temporalmente para reenvíos.");

            if (user.TempCodeAttempts >= maxResends)
            {
                user.TempCodeResendBlockedUntil = now.AddMinutes(blockMinutes);
                await _userData.UpdateAsync(user);
                return VerifyResult.Fail("Máximos reenvíos alcanzados, bloqueado por 1 hora.");
            }

            if (user.TempCodeCreatedAt.HasValue &&
                (now - user.TempCodeCreatedAt.Value).TotalSeconds < cooldownSeconds)
                return VerifyResult.Fail($"Debe esperar {cooldownSeconds} segundos antes de reenviar.");

            // OK → genera y envía
            await GenerateAndSendAsync(user, resetResendAttempts: false);

            user.TempCodeAttempts++;
            await _userData.UpdateAsync(user);

            return VerifyResult.Ok();
        }

    }
}

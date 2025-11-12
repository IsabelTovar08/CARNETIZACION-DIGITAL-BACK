using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Organizational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Operational
{
    public class AttendanceData : BaseData<Attendance>, IAttendanceData
    {
        public AttendanceData(ApplicationDbContext context, ILogger<Attendance> logger)
            : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Attendance>> GetAllAsync()
        {
            return await _context.Set<Attendance>()
                .Include(x => x.AccessPointEntry)
                    .ThenInclude(ap => ap.EventAccessPoints)
                    .ThenInclude(eap => eap.Event)
                .Include(x => x.AccessPointExit)
                    .ThenInclude(ap => ap.EventAccessPoints)
                    .ThenInclude(eap => eap.Event)
                .ToListAsync();
        }

        public async Task<Attendance?> GetOpenAttendanceAsync(int personId, CancellationToken ct = default)
        {
            return await _context.Set<Attendance>()
                .AsNoTracking()
                .Where(a => !a.IsDeleted
                            && a.PersonId == personId
                            && a.TimeOfExit == null)
                .OrderByDescending(a => a.TimeOfEntry)
                .FirstOrDefaultAsync(ct);
        }

        // ✅ Nuevo método sobrescrito para recargar relaciones después de guardar (entrada)
        public override async Task<Attendance> SaveAsync(Attendance entity)
        {
            await base.SaveAsync(entity);

            // 🔹 Recargar la asistencia con sus relaciones completas
            var saved = await _context.Set<Attendance>()
                .Include(a => a.AccessPointEntry)
                .ThenInclude(ap => ap.EventAccessPoints)
                    .ThenInclude(eap => eap.Event)
                .Include(a => a.AccessPointExit)
                .ThenInclude(ap => ap.EventAccessPoints)
                    .ThenInclude(eap => eap.Event)
                .FirstOrDefaultAsync(a => a.Id == entity.Id);

            return saved!;
        }

        public async Task<Attendance> UpdateExitAsync(int id, DateTime timeOfExit, int? accessPointOut, CancellationToken ct = default)
        {
            var entity = await _context.Set<Attendance>()
                .FirstOrDefaultAsync(a => a.Id == id, ct);

            if (entity == null)
                throw new InvalidOperationException($"No se encontró Attendance con Id={id} para actualizar la salida.");

            entity.TimeOfExit = timeOfExit;
            entity.AccessPointOfExit = accessPointOut;

            await _context.SaveChangesAsync(ct);

            // ✅ Recargar el registro con relaciones completas
            var updated = await _context.Set<Attendance>()
                .Include(a => a.AccessPointEntry)
                .ThenInclude(ap => ap.EventAccessPoints)
                    .ThenInclude(eap => eap.Event)
                .Include(a => a.AccessPointExit)
                .ThenInclude(ap => ap.EventAccessPoints)
                    .ThenInclude(eap => eap.Event)
                .FirstOrDefaultAsync(a => a.Id == id, ct);

            return updated!;
        }

        public async Task<(IList<Attendance> Items, int Total)> QueryAsync(
            int? personId, int? eventId, DateTime? fromUtc, DateTime? toUtc,
            string? sortBy, string? sortDir, int page, int pageSize,
            CancellationToken ct = default)
        {
            var q = _context.Set<Attendance>()
                .AsNoTracking()
                .Include(a => a.Person)
                .Include(a => a.AccessPointEntry)
                    .ThenInclude(ap => ap.EventAccessPoints)
                        .ThenInclude(eap => eap.Event)
                .Include(a => a.AccessPointExit)
                    .ThenInclude(ap => ap.EventAccessPoints)
                        .ThenInclude(eap => eap.Event)
                .Where(a => !a.IsDeleted);

            if (personId.HasValue)
                q = q.Where(a => a.PersonId == personId.Value);

            if (eventId.HasValue)
            {
                q = q.Where(a =>
                    (a.AccessPointEntry != null && a.AccessPointEntry.EventAccessPoints.Any(eap => eap.EventId == eventId.Value)) ||
                    (a.AccessPointExit != null && a.AccessPointExit.EventAccessPoints.Any(eap => eap.EventId == eventId.Value))
                );
            }

            if (fromUtc.HasValue)
                q = q.Where(a => a.TimeOfEntry >= fromUtc.Value);
            if (toUtc.HasValue)
                q = q.Where(a => a.TimeOfEntry <= toUtc.Value);

            bool desc = string.Equals(sortDir, "DESC", StringComparison.OrdinalIgnoreCase);
            q = (sortBy ?? "TimeOfEntry") switch
            {
                "TimeOfExit" => (desc ? q.OrderByDescending(a => a.TimeOfExit) : q.OrderBy(a => a.TimeOfExit)),
                "PersonId" => (desc ? q.OrderByDescending(a => a.PersonId) : q.OrderBy(a => a.PersonId)),
                _ => (desc ? q.OrderByDescending(a => a.TimeOfEntry) : q.OrderBy(a => a.TimeOfEntry))
            };

            int total = await q.CountAsync(ct);
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 20 : pageSize;
            int skip = (page - 1) * pageSize;

            var items = await q.Skip(skip).Take(pageSize).ToListAsync(ct);
            return (items, total);
        }

        // ✅ NUEVO MÉTODO — necesario para el AttendanceBusiness
        // Permite hacer Include() desde el negocio sin romper capas
        public IQueryable<Attendance> GetQueryable()
        {
            return _context.Set<Attendance>().AsQueryable();
        }
    }
}

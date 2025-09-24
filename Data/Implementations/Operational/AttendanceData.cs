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
                .Include(x => x.AccessPointExit).
                    ThenInclude(ap => ap.EventAccessPoints)
                        .ThenInclude(eap =>eap.Event)

                .ToListAsync();
        }

        public async Task<Attendance?> GetOpenAttendanceAsync(int personId, CancellationToken ct = default)
        {
            // AsNoTracking para evitar conflictos de tracking cuando luego actualizamos
            return await _context.Set<Attendance>()
                .AsNoTracking()
                .Where(a => !a.IsDeleted
                            && a.PersonId == personId
                            && a.TimeOfExit == null)
                .OrderByDescending(a => a.TimeOfEntry)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<Attendance> UpdateExitAsync(int id, DateTime timeOfExit, int? accessPointOut, CancellationToken ct = default)
        {
            // Obtenemos la entidad "trackeada" y actualizamos sólo los campos de salida
            var entity = await _context.Set<Attendance>()
                .FirstOrDefaultAsync(a => a.Id == id, ct);

            if (entity == null)
                throw new InvalidOperationException($"No se encontró Attendance con Id={id} para actualizar la salida.");

            entity.TimeOfExit = timeOfExit;
            entity.AccessPointOfExit = accessPointOut;

            await _context.SaveChangesAsync(ct);
            return entity;
        }

        //NUEVO: consulta con filtros y paginación
        public async Task<(IList<Attendance> Items, int Total)> QueryAsync(
            int? personId, int? eventId, DateTime? fromUtc, DateTime? toUtc,
            string? sortBy, string? sortDir, int page, int pageSize,
            CancellationToken ct = default)
        {
            var q = _context.Set<Attendance>()
                .AsNoTracking() // ⚡ performance para lectura
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

            // Filtra por evento (entrada o salida)
            if (eventId.HasValue)
            {
                q = q.Where(a =>
                 (a.AccessPointEntry != null && a.AccessPointEntry.EventAccessPoints.Any(eap => eap.EventId == eventId.Value)) ||
                 (a.AccessPointExit != null && a.AccessPointExit.EventAccessPoints.Any(eap => eap.EventId == eventId.Value))
             );

            }

            if (fromUtc.HasValue) q = q.Where(a => a.TimeOfEntry >= fromUtc.Value);
            if (toUtc.HasValue) q = q.Where(a => a.TimeOfEntry <= toUtc.Value);

            // Orden
            bool desc = string.Equals(sortDir, "DESC", StringComparison.OrdinalIgnoreCase);
            q = (sortBy ?? "TimeOfEntry") switch
            {
                "TimeOfExit" => (desc ? q.OrderByDescending(a => a.TimeOfExit) : q.OrderBy(a => a.TimeOfExit)),
                "PersonId" => (desc ? q.OrderByDescending(a => a.PersonId) : q.OrderBy(a => a.PersonId)),
                _ => (desc ? q.OrderByDescending(a => a.TimeOfEntry) : q.OrderBy(a => a.TimeOfEntry))
            };

            int total = await q.CountAsync(ct);

            // Paginación (1-based)
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 20 : pageSize;
            int skip = (page - 1) * pageSize;

            var items = await q.Skip(skip).Take(pageSize).ToListAsync(ct);
            return (items, total);
        }
    }
}

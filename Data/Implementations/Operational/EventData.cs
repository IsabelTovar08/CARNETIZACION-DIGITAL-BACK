using AutoMapper;
using Data.Classes.Base;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Operational;
using Entity.Models.Organizational;
using Entity.Models.Organizational.Structure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations.Operational
{
    public class EventData : BaseData<Event>, IEventData
    {
        public EventData(ApplicationDbContext context, ILogger<Event> logger) : base(context, logger) { }

        // ✅ NUEVO MÉTODO para cumplir con la interfaz IEventData
        public IQueryable<Event> GetQueryable()
        {
            return _context.Set<Event>().AsQueryable();
        }

        public async Task<Event?> GetEventWithDetailsAsync(int eventId)
        {
            var ev = await _context.Set<Event>()
                .AsSplitQuery()

                .Include(e => e.Schedule)
                .Include(e => e.EventType)

                //Incluye el estado (Status)
                .Include(e => e.Status)

                .Include(e => e.EventAccessPoints)
                    .ThenInclude(eap => eap.AccessPoint)
                        .ThenInclude(ap => ap.AccessPointType)

                .Include(e => e.EventTargetAudiences)
                    .ThenInclude(a => a.Profile)

                .Include(e => e.EventTargetAudiences)
                    .ThenInclude(a => a.OrganizationalUnit)

                .Include(e => e.EventTargetAudiences)
                    .ThenInclude(a => a.InternalDivision)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            return ev;
        }


        /// <summary>
        /// Para listar todos los eventos con su informacion completa
        /// </summary>
        /// <returns></returns>
        public async Task<List<Event>> GetAllEventsWithDetailsAsync()
        {
          
               var ev = await _context.Set<Event>()

                .AsSplitQuery()
                .Include(e => e.EventType)
                .Include(e => e.Schedule)
                .Include(e => e.Status)

               .Include(e => e.EventAccessPoints)
                    .ThenInclude(eap => eap.AccessPoint)
                        .ThenInclude(ap => ap.AccessPointType)

                .Include(e => e.EventTargetAudiences)
                    .ThenInclude(a => a.Profile)

                .Include(e => e.EventTargetAudiences)
                    .ThenInclude(a => a.OrganizationalUnit)

                .Include(e => e.EventTargetAudiences)
                    .ThenInclude(a => a.InternalDivision)

                .ToListAsync();

            return ev;
        }


        public async Task<Event> SaveFullEventAsync(
            Event ev,
            IEnumerable<AccessPoint> accessPoints,
            IEnumerable<EventTargetAudience> audiences)
        {
            await _context.Events.AddAsync(ev);
            await _context.SaveChangesAsync();

            if (accessPoints != null && accessPoints.Any())
            {
                await _context.AccessPoints.AddRangeAsync(accessPoints);
                await _context.SaveChangesAsync();

                var links = accessPoints.Select(ap => new EventAccessPoint
                {
                    EventId = ev.Id,
                    AccessPointId = ap.Id
                });

                await _context.EventAccessPoints.AddRangeAsync(links);
            }

            if (audiences != null && audiences.Any())
            {
                foreach (var au in audiences)
                    au.EventId = ev.Id;

                await _context.EventTargetAudiences.AddRangeAsync(audiences);
            }

            await _context.SaveChangesAsync();
            return ev;
        }

        public async Task BulkInsertEventAccessPointsAsync(IEnumerable<EventAccessPoint> links)
        {
            await _context.EventAccessPoints.AddRangeAsync(links);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAccessPointsAsync(IEnumerable<AccessPoint> accessPoints)
        {
            await _context.Set<AccessPoint>().AddRangeAsync(accessPoints);
            await _context.SaveChangesAsync();
        }

        public async Task BulkInsertAccessPointsAsync(IEnumerable<AccessPoint> accessPoints)
        {
            await _context.AccessPoints.AddRangeAsync(accessPoints);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetAvailableEventsCountAsync()
        {
            try
            {
                var now = DateTime.UtcNow;

                var total = await _context.Set<Event>()
                    .AsNoTracking()
                    .Where(e => !e.IsDeleted
                                && e.StatusId == 1
                                && (e.EventEnd == null || e.EventEnd >= now))
                    .CountAsync();

                return total;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el número de eventos disponibles");
                throw;
            }
        }

        public async Task DeleteEventAccessPointsByEventIdAsync(int eventId)
        {
            var links = _context.EventAccessPoints.Where(eap => eap.EventId == eventId);
            if (links.Any())
            {
                _context.EventAccessPoints.RemoveRange(links);
                await _context.SaveChangesAsync();
            }
        }

        // metodo para el servicio que finaliza eventos automáticamente
        public async Task<List<Event>> GetEventsToFinalizeAsync(DateTime now)
        {
            var localNow = DateTime.SpecifyKind(now, DateTimeKind.Local);

            return await _context.Set<Event>()
                .Where(e => (e.StatusId == 1 || e.StatusId == 8)
                    && e.EventEnd <= localNow
                    && !e.IsDeleted)
                .ToListAsync();
        }

        // metodo para el servicio que verifica y actualiza el estado de eventos "en curso"
        public async Task<IEnumerable<Event>> GetActiveEventsAsync()
        {
            return await _context.Set<Event>()
                .Include(e => e.Schedule)
                .Where(e => e.StatusId == 1 || e.StatusId == 8)
                .ToListAsync();
        }

        // =============================================================
        //  implementación real para filtros personalizados
        // =============================================================
        public async Task<List<Event>> ToListAsync(IQueryable<Event> query)
        {
            return await query
                .Include(e => e.Status)
                .Include(e => e.EventType)
                .ToListAsync();
        }
    }
}

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
        public EventData(ApplicationDbContext context, ILogger<Event> logger) : base(context, logger){}
        public async Task<Event?> GetEventWithDetailsAsync(int eventId)
        {
            var ev = await _context.Set<Event>()
                .AsSplitQuery()
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


        public async Task<Event> SaveFullEventAsync(
        Event ev,
        IEnumerable<AccessPoint> accessPoints,
        IEnumerable<EventTargetAudience> audiences)
        {
            // 1. Guardar evento
            await _context.Events.AddAsync(ev);
            await _context.SaveChangesAsync();

            // 2. Guardar access points
            if (accessPoints != null && accessPoints.Any())
            {
                await _context.AccessPoints.AddRangeAsync(accessPoints);
                await _context.SaveChangesAsync();

                // Crear relaciones en EventAccessPoints
                var links = accessPoints.Select(ap => new EventAccessPoint
                {
                    EventId = ev.Id,
                    AccessPointId = ap.Id
                });

                await _context.EventAccessPoints.AddRangeAsync(links);
            }

            // 3. Guardar audiencias
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

    }

}

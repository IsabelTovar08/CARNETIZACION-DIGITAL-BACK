using Data.Classes.Base;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Operational;
using Entity.Models.Organizational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations.Operational
{
    public class EventAccessPointData : BaseData<EventAccessPoint>, IEventAccessPointData
    {
        public EventAccessPointData(ApplicationDbContext context, ILogger<EventAccessPoint> logger) : base(context, logger)
        {
        }

        public override Task<EventAccessPoint> SaveAsync(EventAccessPoint entity)
        {
            entity.QrCodeKey = Guid.NewGuid().ToString();
            return base.SaveAsync(entity);
        }
        public async override Task<IEnumerable<EventAccessPoint>> GetAllAsync()
        {
            return await _context.Set<EventAccessPoint>()
                .Include(x => x.AccessPoint)
                .Include(x => x.Event)
                .ToListAsync();
        }

        public async Task<bool> ExistsDuplicateAsync(int eventId, int accessPointId)
        {
            return await _context.EventAccessPoints
                .AsNoTracking()
                .AnyAsync(x =>
                    x.EventId == eventId &&
                    x.AccessPointId == accessPointId &&
                    !x.IsDeleted);
        }

        /// <summary>
        /// Obtiene un EventAccessPoint usando el QrCodeKey.
        /// </summary>
        public async Task<EventAccessPoint?> GetByQrKeyAsync(string qrKey)
        {
            return await _context.EventAccessPoints
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.QrCodeKey == qrKey && !x.IsDeleted);
        }

    }
}
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

        public async override Task<IEnumerable<EventAccessPoint>> GetAllAsync()
        {
            return await _context.Set<EventAccessPoint>()
                .Include(x => x.AccessPoint)
                .Include(x => x.Event)
                .ToListAsync();
        }
    }
}

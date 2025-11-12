using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Organizational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Operational
{
    public class EventTargetAudienceData : BaseData<EventTargetAudience>, IEventTargetAudienceData
    {
        public EventTargetAudienceData(ApplicationDbContext context, ILogger<EventTargetAudience> logger)
            : base(context, logger)
        {
        }

        public override async Task<IEnumerable<EventTargetAudience>> GetAllAsync()
        {
            return await _context.Set<EventTargetAudience>().Include(x => x.CustomType).Include(x => x.Event).ToListAsync();
        }

        public async Task BulkInsertAsync(IEnumerable<EventTargetAudience> audiences)
        {
            await _context.EventTargetAudiences.AddRangeAsync(audiences);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByEventIdAsync(int eventId)
        {
            var audiences = _context.EventTargetAudiences.Where(a => a.EventId == eventId);
            if (audiences.Any())
            {
                _context.EventTargetAudiences.RemoveRange(audiences);
                await _context.SaveChangesAsync();
            }
        }


    }
}

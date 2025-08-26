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
    }
}

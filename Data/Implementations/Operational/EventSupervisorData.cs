using Data.Classes.Base;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Operational;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations.Operational
{
    public class EventSupervisorData : BaseData<EventSupervisor>, IEventSupervisorData
    {
        public EventSupervisorData(ApplicationDbContext context, ILogger<EventSupervisor> logger) : base(context, logger)
        { }

        public async Task BulkInsertAsync(IEnumerable<EventSupervisor> supervisors)
        {
            await _context.EventSupervisors.AddRangeAsync(supervisors);
            await _context.SaveChangesAsync();
        }
    }


}

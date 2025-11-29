using Data.Classes.Base;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Operational;
using Microsoft.EntityFrameworkCore;
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


        /// <summary>
        /// Agrega supervisor a un evento existente
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<List<EventSupervisor>> GetSupervisorsWithUserAsync(int eventId)
        {
            return await _context.EventSupervisors
                .Where(x => x.EventId == eventId && !x.IsDeleted)
                .Include(x => x.User)
                    .ThenInclude(u => u.Person)
                .ToListAsync();
        }

        public async Task<bool> SupervisorExistsAsync(int eventId, int userId)
        {
            return await _context.EventSupervisors
                .AnyAsync(s => s.EventId == eventId && s.UserId == userId && !s.IsDeleted);
        }
    }


}

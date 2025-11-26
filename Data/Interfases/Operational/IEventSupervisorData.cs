using Data.Interfases;
using Entity.Models.Operational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfases.Operational
{
    public interface IEventSupervisorData : ICrudBase<EventSupervisor>
    {
        Task BulkInsertAsync(IEnumerable<EventSupervisor> supervisors);

        Task<List<EventSupervisor>> GetSupervisorsWithUserAsync(int eventId);

    }
}

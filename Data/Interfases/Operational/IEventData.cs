using Entity.DTOs.Operational;
using Entity.Models.Operational;
using Entity.Models.Organizational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfases.Operational
{
    public interface IEventData : ICrudBase<Event>
    {
        Task<Event?> GetEventWithDetailsAsync(int eventId);
        Task<Event> SaveFullEventAsync(
             Event ev,
             IEnumerable<AccessPoint> accessPoints,
             IEnumerable<EventTargetAudience> audiences);
        Task BulkInsertEventAccessPointsAsync(IEnumerable<EventAccessPoint> links);
        Task BulkInsertAccessPointsAsync(IEnumerable<AccessPoint> accessPoints);
        Task SaveAccessPointsAsync(IEnumerable<AccessPoint> accessPoints);

    }
}

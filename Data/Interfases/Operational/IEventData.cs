using Entity.DTOs.Operational;
using Entity.Models.Operational;
using Entity.Models.Organizational;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Interfases.Operational
{
    public interface IEventData : ICrudBase<Event>
    {
        IQueryable<Event> GetQueryable();

        Task<Event?> GetEventWithDetailsAsync(int eventId);

        Task<Event> SaveFullEventAsync(
            Event ev,
            IEnumerable<AccessPoint> accessPoints,
            IEnumerable<EventTargetAudience> audiences);

        Task BulkInsertEventAccessPointsAsync(IEnumerable<EventAccessPoint> links);

        Task BulkInsertAccessPointsAsync(IEnumerable<AccessPoint> accessPoints);

        Task SaveAccessPointsAsync(IEnumerable<AccessPoint> accessPoints);

        Task<int> GetAvailableEventsCountAsync();

        Task DeleteEventAccessPointsByEventIdAsync(int eventId);

        Task<List<Event>> GetEventsToFinalizeAsync(DateTime now);

        Task<IEnumerable<Event>> GetActiveEventsAsync();

        // =============================================================
        // 🚀 NUEVO — requerido para filtros personalizados en Business
        // =============================================================
        /// <summary>
        /// Convierte un IQueryable<Event> en una lista asincrónicamente.
        /// </summary>
        Task<List<Event>> ToListAsync(IQueryable<Event> query);
    }
}

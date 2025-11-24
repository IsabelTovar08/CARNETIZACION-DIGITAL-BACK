using Entity.DTOs.Operational;
using Entity.DTOs.Operational.Response;
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

        /// <summary>
        /// Para listar los eventos con toda la informacion
        /// </summary>
        /// <returns></returns>
        Task<List<Event>> GetAllEventsWithDetailsAsync();

        // =============================================================
        // 🚀 NUEVO — requerido para filtros personalizados en Business
        // =============================================================
        /// <summary>
        /// Convierte un IQueryable<Event> en una lista asincrónicamente.
        /// </summary>
        Task<List<Event>> ToListAsync(IQueryable<Event> query);

        /// <summary>
        ///  para insertar mas de una jornada en un evento
        /// </summary>
        /// <param name="links"></param>
        /// <returns></returns>
        Task BulkInsertEventSchedulesAsync(List<EventSchedule> links);

        /// <summary>
        /// para eliminar la relacion de las jornadas con los eventos
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        Task DeleteEventSchedulesByEventIdAsync(int eventId);

        /// <summary>
        /// Obtiene el conteo de eventos por tipo de evento.
        /// </summary>
        /// <returns></returns>
        Task<List<EventTypeCountDtoResponse>> GetEventTypeCountsAsync();

        /// <summary>
        /// Para obtener los eventos con mayor asistencia.
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        Task<List<EventAttendanceTopDtoResponse>> GetTopEventsByTypeAsync(int eventTypeId, int top = 5);


    }
}

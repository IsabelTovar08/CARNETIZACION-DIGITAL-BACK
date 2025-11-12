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
        /// <summary>
        /// Devuelve un IQueryable de eventos para consultas personalizadas
        /// (por ejemplo, se usa desde AttendanceBusiness para buscar por código del evento).
        /// </summary>
        IQueryable<Event> GetQueryable();

        /// <summary>
        /// Obtiene un evento con sus accesos, audiencias y demás relaciones.
        /// </summary>
        /// <param name="eventId">ID del evento a buscar.</param>
        Task<Event?> GetEventWithDetailsAsync(int eventId);

        /// <summary>
        /// Guarda un evento completo con sus AccessPoints y Audiences asociados.
        /// </summary>
        Task<Event> SaveFullEventAsync(
            Event ev,
            IEnumerable<AccessPoint> accessPoints,
            IEnumerable<EventTargetAudience> audiences);

        /// <summary>
        /// Inserta en bloque los vínculos EventAccessPoint (relación evento ↔ punto de acceso).
        /// </summary>
        Task BulkInsertEventAccessPointsAsync(IEnumerable<EventAccessPoint> links);

        /// <summary>
        /// Inserta múltiples AccessPoints relacionados a eventos.
        /// </summary>
        Task BulkInsertAccessPointsAsync(IEnumerable<AccessPoint> accessPoints);

        /// <summary>
        /// Guarda o actualiza una colección de AccessPoints en la base de datos.
        /// </summary>
        Task SaveAccessPointsAsync(IEnumerable<AccessPoint> accessPoints);

        /// <summary>
        /// Retorna el número de eventos disponibles (no eliminados, activos y dentro de fecha).
        /// </summary>
        Task<int> GetAvailableEventsCountAsync();

        Task DeleteEventAccessPointsByEventIdAsync(int eventId);

        /// <summary>
        /// para el servicio que finaliza eventos automáticamente:
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        Task<List<Event>> GetEventsToFinalizeAsync(DateTime now);

        /// <summary>
        /// Para el servicio que verifica y actualiza el estado de eventos "en curso":
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Event>> GetActiveEventsAsync();

        /// <summary>
        /// Para listar los eventos con toda la informacion
        /// </summary>
        /// <returns></returns>
        Task<List<Event>> GetAllEventsWithDetailsAsync();
    }
}

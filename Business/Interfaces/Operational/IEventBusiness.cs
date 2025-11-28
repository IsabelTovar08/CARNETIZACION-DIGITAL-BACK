using Business.Interfases;
using Entity.DTOs.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Specifics;   // ✅ NECESARIO PARA EventFilterDto

namespace Business.Interfaces.Operational
{
    public interface IEventBusiness : IBaseBusiness<Event, EventDtoResponse, EventDtoResponse>
    {
        Task<int> CreateEventAsync(CreateEventRequest dto);
        Task<EventDetailsDtoResponse?> GetEventFullDetailsAsync(int eventId);

        /// <summary>
        /// Retorna el número de eventos disponibles
        /// </summary>
        Task<int> GetAvailableEventsCountAsync();

        Task<int> UpdateEventAsync(EventDtoRequest dto);

        Task<bool> FinalizeEventAsync(int eventId);

        /// <summary>
        /// Para que el servicio que verifica y actualiza el estado de eventos "en curso"
        /// </summary>
        Task CheckAndUpdateEventStatusAsync(int eventId);

        /// <summary>
        /// Para llamar todos los eventos y mostrar toda la informacion
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EventDetailsDtoResponse>> GetFullListAsync();

        Task<IEnumerable<EventDtoResponse>> FilterAsync(EventFilterDto filters);

        /// <summary>
        /// Para obtener el conteo de eventos por tipo de evento
        /// </summary>
        /// <returns></returns>
        Task<List<EventTypeCountDtoResponse>> GetEventTypeCountsAsync();


        /// <summary>
        /// Para obtener los 5 eventos con mas asistencia
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        Task<List<EventAttendanceTopDtoResponse>> GetTopEventsByTypeAsync(int eventTypeId, int top = 5);

    }
}

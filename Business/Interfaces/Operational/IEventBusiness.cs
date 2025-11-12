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

        // ============================================================
        // 🚀 NUEVO: FILTRO POR ESTADO, TIPO Y PÚBLICO/PRIVADO
        // ============================================================
        Task<IEnumerable<EventDtoResponse>> FilterAsync(EventFilterDto filters);
    }
}

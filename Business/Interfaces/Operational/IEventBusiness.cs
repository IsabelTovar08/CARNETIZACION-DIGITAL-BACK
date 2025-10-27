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
    }
}

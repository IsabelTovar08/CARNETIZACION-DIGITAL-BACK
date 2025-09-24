using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Data.Implementations.Operational;
using Data.Interfases;
using Data.Interfases.Operational;
using Entity.DTOs.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;
using Entity.Models.Organizational;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Helpers;

namespace Business.Implementations.Operational
{
    public class EventBusiness : BaseBusiness<Event, EventDtoResponse, EventDtoResponse>, IEventBusiness
    {

        public readonly IEventData _data;
        private readonly IEventTargetAudienceData _audienceRepo;
        private readonly IMapper _mapper;
        private readonly IAccessPointData _apData;

        public EventBusiness(IEventData data, IEventTargetAudienceData audienceRepo, IAccessPointData apData, ILogger<Event> logger, IMapper mapper) : base(data, logger, mapper)
        {
            _data = data;
            _audienceRepo = audienceRepo;
            _apData = apData;
            _mapper = mapper;
        }

        public async Task<int> CreateEventAsync(CreateEventRequest dto)
        {
            // 1) Crear evento
            var ev = _mapper.Map<Event>(dto.Event);
            var savedEvent = await _data.SaveAsync(ev);

            // 2) Crear AccessPoints nuevos si vienen en el DTO
            var createdAccessPoints = new List<AccessPoint>();
            if (dto.AccessPoints?.Any() == true)
            {
                createdAccessPoints = dto.AccessPoints
                    .Select(apDto => _mapper.Map<AccessPoint>(apDto))
                    .ToList();

                // Guardar primero los access points (así obtienen su Id)
                await _apData.BulkInsertAsync(createdAccessPoints);

                // Crear vínculos Event ↔ AccessPoint
                var links = createdAccessPoints.Select(ap => new EventAccessPoint
                {
                    EventId = savedEvent.Id,
                    AccessPointId = ap.Id
                }).ToList();

                await _data.BulkInsertEventAccessPointsAsync(links);
            }

            // 3) Mapear audiencias
            var audiences = new List<EventTargetAudience>();

            if (dto.ProfileIds?.Any() == true)
            {
                audiences.AddRange(dto.ProfileIds.Select(pid => new EventTargetAudience
                {
                    TypeId = 1,
                    ProfileId = pid,
                    EventId = savedEvent.Id
                }));
            }

            if (dto.OrganizationalUnitIds?.Any() == true)
            {
                audiences.AddRange(dto.OrganizationalUnitIds.Select(ouid => new EventTargetAudience
                {
                    TypeId = 2,
                    OrganizationalUnitId = ouid,
                    EventId = savedEvent.Id
                }));
            }

            if (dto.InternalDivisionIds?.Any() == true)
            {
                audiences.AddRange(dto.InternalDivisionIds.Select(did => new EventTargetAudience
                {
                    TypeId = 3,
                    InternalDivisionId = did,
                    EventId = savedEvent.Id
                }));
            }

            if (audiences.Any())
                await _audienceRepo.BulkInsertAsync(audiences);

            return savedEvent.Id;
        }



        public async Task<EventDetailsDtoResponse?> GetEventFullDetailsAsync(int eventId)
        {
            var entity = await _data.GetEventWithDetailsAsync(eventId);

            return _mapper.Map<EventDetailsDtoResponse>(entity);
        }
    }
}

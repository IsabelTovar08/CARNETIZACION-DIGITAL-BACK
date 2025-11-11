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
using QRCoder;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exeptions;
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
            // 1) Mapear evento desde DTO
            var ev = _mapper.Map<Event>(dto.Event);

            // Generar QR único en Base64 y asignarlo al evento antes de persistir
            string qrData = $"EVENT-{Guid.NewGuid()}-{ev.Code}-{DateTime.UtcNow:yyyyMMddHHmmss}";
            using var qrGen = new QRCodeGenerator();
            using var qrCodeData = qrGen.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrCodeData);
            using var bitmap = qrCode.GetGraphic(20);
            using var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ev.QrCodeBase64 = Convert.ToBase64String(ms.ToArray());

            // Guardar evento (ya con QrCodeBase64)
            var savedEvent = await _data.SaveAsync(ev);

            // Crear AccessPoints nuevos si vienen en el DTO
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

            // Mapear audiencias
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

        public async Task<int> UpdateEventAsync(EventDtoRequest dto)
        {
            try
            {
                // 1️⃣ Buscar el evento existente con sus relaciones
                var existingEvent = await _data.GetEventWithDetailsAsync(dto.Id);

                if (existingEvent == null)
                    throw new EntityNotFoundException($"No se encontró el evento con Id {dto.Id}");

                // 2️⃣ Actualizar solo los campos principales
                existingEvent.Name = dto.Name;
                existingEvent.Description = dto.Description;
                existingEvent.ScheduleDate = dto.ScheduleDate;
                existingEvent.ScheduleTime = dto.ScheduleTime;
                existingEvent.ScheduleId = dto.SheduleId;
                existingEvent.EventTypeId = dto.EventTypeId;
                existingEvent.StatusId = dto.StatusId;
                existingEvent.IsPublic = dto.Ispublic;
                existingEvent.UpdateAt = DateTime.UtcNow;

                // 3️⃣ AccessPoints (actualiza vínculos)
                if (dto.AccessPoints != null && dto.AccessPoints.Any())
                {
                    await _data.DeleteEventAccessPointsByEventIdAsync(existingEvent.Id);

                    var newLinks = dto.AccessPoints.Select(apId => new EventAccessPoint
                    {
                        EventId = existingEvent.Id,
                        AccessPointId = apId
                    }).ToList();

                    await _data.BulkInsertEventAccessPointsAsync(newLinks);
                }

                // 4️⃣ Audiencias (Profiles / Units / Divisions)
                await _audienceRepo.DeleteByEventIdAsync(existingEvent.Id);

                var newAudiences = new List<EventTargetAudience>();

                if (dto.ProfileIds?.Any() == true)
                {
                    newAudiences.AddRange(dto.ProfileIds.Select(pid => new EventTargetAudience
                    {
                        TypeId = 1,
                        ProfileId = pid,
                        EventId = existingEvent.Id
                    }));
                }

                if (dto.OrganizationalUnitIds?.Any() == true)
                {
                    newAudiences.AddRange(dto.OrganizationalUnitIds.Select(ouid => new EventTargetAudience
                    {
                        TypeId = 2,
                        OrganizationalUnitId = ouid,
                        EventId = existingEvent.Id
                    }));
                }

                if (dto.InternalDivisionIds?.Any() == true)
                {
                    newAudiences.AddRange(dto.InternalDivisionIds.Select(did => new EventTargetAudience
                    {
                        TypeId = 3,
                        InternalDivisionId = did,
                        EventId = existingEvent.Id
                    }));
                }

                if (newAudiences.Any())
                    await _audienceRepo.BulkInsertAsync(newAudiences);

                // 5️⃣ Guardar cambios principales
                await _data.UpdateAsync(existingEvent);

                return existingEvent.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el evento con Id {dto.Id}");
                throw new ExternalServiceException("Error al actualizar el evento", ex.Message);
            }
        }

        public async Task<EventDetailsDtoResponse?> GetEventFullDetailsAsync(int eventId)
        {
            var entity = await _data.GetEventWithDetailsAsync(eventId);

            return _mapper.Map<EventDetailsDtoResponse>(entity);
        }


        /// <summary>
        /// Retorna el número de eventos disponibles
        /// </summary>
        public async Task<int> GetAvailableEventsCountAsync()
        {
            try
            {
                var total = await _data.GetAvailableEventsCountAsync();

                if (total < 0)
                    throw new InvalidOperationException("El número de eventos no puede ser negativo");

                return total;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al obtener eventos disponibles");
                throw; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en Business al obtener eventos disponibles");
                throw; 
            }
        }
    }
}

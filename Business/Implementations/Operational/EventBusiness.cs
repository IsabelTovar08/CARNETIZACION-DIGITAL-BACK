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
using Microsoft.Extensions.Logging;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Exeptions;

namespace Business.Implementations.Operational
{
    public class EventBusiness : BaseBusiness<Event, EventDtoResponse, EventDtoResponse>, IEventBusiness
    {
        private readonly IEventData _data;
        private readonly IEventTargetAudienceData _audienceRepo;
        private readonly IAccessPointData _apData;
        private readonly IMapper _mapper;
        private readonly ILogger<Event> _logger;

        public EventBusiness(
            IEventData data,
            IEventTargetAudienceData audienceRepo,
            IAccessPointData apData,
            ILogger<Event> logger,
            IMapper mapper
        ) : base(data, logger, mapper)
        {
            _data = data;
            _audienceRepo = audienceRepo;
            _apData = apData;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene los detalles completos del evento.
        /// </summary>
        public async Task<EventDetailsDtoResponse?> GetEventFullDetailsAsync(int eventId)
        {
            var entity = await _data.GetEventWithDetailsAsync(eventId);
            return _mapper.Map<EventDetailsDtoResponse>(entity);
        }

        /// <summary>
        /// Retorna el número de eventos disponibles.
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

        /// <summary>
        /// Finaliza un evento manualmente.
        /// </summary>
        public async Task<bool> FinalizeEventAsync(int eventId)
        {
            var existingEvent = await _data.GetByIdAsync(eventId);

            if (existingEvent == null)
                throw new EntityNotFoundException($"Evento con Id {eventId} no encontrado.");

            existingEvent.StatusId = 10; 
            existingEvent.IsPublic = false;
            existingEvent.UpdateAt = DateTime.UtcNow;

            await _data.UpdateAsync(existingEvent);
            return true;
        }

        /// <summary>
        /// Para listar todos los eventos con su informacion completa
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EventDetailsDtoResponse>> GetFullListAsync()
        {
            var events = await _data.GetAllEventsWithDetailsAsync();
            return _mapper.Map<IEnumerable<EventDetailsDtoResponse>>(events);
        }

        /// <summary>
        /// Crea un evento con accesos, audiencias y genera un código QR.
        /// </summary>
        public async Task<int> CreateEventAsync(CreateEventRequest dto)
        {
            try
            {
                // Crear el evento
                var ev = _mapper.Map<Event>(dto.Event);
                var savedEvent = await _data.SaveAsync(ev);

                // Crear puntos de acceso (AccessPoints)
                var createdAccessPoints = new List<AccessPoint>();
                if (dto.AccessPoints?.Any() == true)
                {
                    createdAccessPoints = dto.AccessPoints
                        .Select(apDto => new AccessPoint
                        {
                            Name = apDto.Name,
                            Description = apDto.Description,
                            TypeId = apDto.TypeId,
                            IsDeleted = false
                        })
                        .ToList();

                    await _apData.BulkInsertAsync(createdAccessPoints);

                    // Crear vínculos (EventAccessPoints)
                    var links = createdAccessPoints.Select(ap => new EventAccessPoint
                    {
                        EventId = savedEvent.Id,
                        AccessPointId = ap.Id
                    }).ToList();

                    await _data.BulkInsertEventAccessPointsAsync(links);
                }

                // Generar código QR real
                string firstAccessPoint = createdAccessPoints.FirstOrDefault()?.Name ?? "General";
                string qrContent = $"EVT|{savedEvent.Id}|{savedEvent.Name}|{firstAccessPoint}";
                savedEvent.QrCodeBase64 = GenerateQrCodeBase64(qrContent);
                await _data.UpdateAsync(savedEvent);

                // Crear audiencias (Perfiles, Unidades, Divisiones)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear evento con access points");
                throw;
            }
        }

        /// <summary>
        /// Actualiza el estado del evento automáticamente a "En curso" si coincide con su horario.
        /// </summary>
        public async Task CheckAndUpdateEventStatusAsync(int eventId)
        {
            var ev = await _data.GetEventWithDetailsAsync(eventId);
            if (ev == null) return;

            // Fecha y hora actual
            var now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);


            // Validar rango de fechas del evento
            if (ev.EventStart.HasValue && ev.EventEnd.HasValue && ev.EventStart <= now && ev.EventEnd >= now)
            {
                // Si tiene horario asociado (Schedule)
                if (ev.Schedule != null)
                {
                    var today = now.DayOfWeek.ToString();
                    var scheduleDays = ev.Schedule.Days?.Split(',').Select(d => d.Trim()) ?? Enumerable.Empty<string>();

                    // Verificar si hoy aplica
                    if (scheduleDays.Contains(today, StringComparer.OrdinalIgnoreCase))
                    {
                        var startTime = ev.Schedule.StartTime;
                        var endTime = ev.Schedule.EndTime;

                        // Hora actual dentro del rango
                        if (now.TimeOfDay >= startTime && now.TimeOfDay <= endTime)
                        {
                            // Si no está ya en curso
                            if (ev.StatusId != 8) // 8 = "En curso"
                            {
                                ev.StatusId = 8;
                                ev.UpdateAt = DateTime.Now;
                                await _data.UpdateAsync(ev);

                                _logger.LogInformation($"Evento {ev.Id} marcado como 'En curso'.");
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Actualiza un evento existente con sus relaciones.
        /// </summary>
        public async Task<int> UpdateEventAsync(EventDtoRequest dto)
        {
            try
            {
                // Buscar el evento existente con sus relaciones
                var existingEvent = await _data.GetEventWithDetailsAsync(dto.Id);
                if (existingEvent == null)
                    throw new EntityNotFoundException($"No se encontró el evento con Id {dto.Id}");

                // Actualizar solo los campos principales
                existingEvent.Name = dto.Name;
                existingEvent.Description = dto.Description;
                existingEvent.EventStart = dto.EventStart;
                existingEvent.EventEnd = dto.EventEnd;
                existingEvent.ScheduleId = dto.ScheduleId;
                existingEvent.EventTypeId = dto.EventTypeId;
                existingEvent.StatusId = dto.StatusId;
                existingEvent.IsPublic = dto.Ispublic;
                existingEvent.UpdateAt = DateTime.Now;
   
                // Actualizar AccessPoints (vínculos)
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

                // Actualizar audiencias
                await _audienceRepo.DeleteByEventIdAsync(existingEvent.Id);
                await CreateEventAudiencesAsync(existingEvent.Id, dto.ProfileIds, dto.OrganizationalUnitIds, dto.InternalDivisionIds);

                // Guardar cambios
                await _data.UpdateAsync(existingEvent);

                return existingEvent.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el evento con Id {dto.Id}");
                throw new ExternalServiceException("Error al actualizar el evento", ex.Message);
            }
        }

        #region Métodos Privados

        /// <summary>
        /// Genera un código QR en formato Base64.
        /// </summary>
        private string GenerateQrCodeBase64(string content)
        {
            using var qrGen = new QRCodeGenerator();
            using var qrData = qrGen.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrData);
            using var bitmap = qrCode.GetGraphic(6);
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// Crea las audiencias del evento (perfiles, unidades, divisiones).
        /// </summary>
        private async Task CreateEventAudiencesAsync(
            int eventId,
            IEnumerable<int>? profileIds,
            IEnumerable<int>? organizationalUnitIds,
            IEnumerable<int>? internalDivisionIds)
        {
            var audiences = new List<EventTargetAudience>();

            if (profileIds?.Any() == true)
            {
                audiences.AddRange(profileIds.Select(pid => new EventTargetAudience
                {
                    TypeId = 1,
                    ProfileId = pid,
                    EventId = eventId
                }));
            }

            if (organizationalUnitIds?.Any() == true)
            {
                audiences.AddRange(organizationalUnitIds.Select(ouid => new EventTargetAudience
                {
                    TypeId = 2,
                    OrganizationalUnitId = ouid,
                    EventId = eventId
                }));
            }

            if (internalDivisionIds?.Any() == true)
            {
                audiences.AddRange(internalDivisionIds.Select(did => new EventTargetAudience
                {
                    TypeId = 3,
                    InternalDivisionId = did,
                    EventId = eventId
                }));
            }

            if (audiences.Any())
                await _audienceRepo.BulkInsertAsync(audiences);
        }

        #endregion
    }
}
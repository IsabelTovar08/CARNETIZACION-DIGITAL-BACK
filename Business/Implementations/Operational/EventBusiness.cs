using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Business.Services.Supervisors;
using Data.Implementations.Operational;
using Data.Interfases;
using Data.Interfases.Operational;
using Entity.DTOs.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Specifics; 
using Entity.Models.Operational;
using Entity.Models.Organizational;
using Infrastructure.Notifications.Interfases;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Exeptions;
using Utilities.Helpers;
using Utilities.Notifications.Implementations;
using Utilities.Notifications.Interfases;
using static QRCoder.PayloadGenerator;
using static Utilities.Helpers.BarcodeHelper;

namespace Business.Implementations.Operational
{
    public class EventBusiness : BaseBusiness<Event, EventDtoResponse, EventDtoResponse>, IEventBusiness
    {
        private readonly IEventData _data;
        private readonly IEventTargetAudienceData _audienceRepo;
        private readonly IAccessPointData _apData;
        private readonly IEventSupervisorData _supervisorData;
        private readonly IMapper _mapper;
        private readonly ILogger<Event> _logger;
        private readonly IAttendanceData _attendanceData;
        private readonly IEventAttendancePdfService _pdfGenerator;
        //private readonly IEventAttendancePdfService _pdfService;
        private readonly INotify _notifier;
        private readonly IEmailAttachmentSender _emailAttachmentSender;


        public EventBusiness(
            IEventData data,
            IEventTargetAudienceData audienceRepo,
            IAttendanceData attendanceData,
            IAccessPointData apData,
            IEventSupervisorData supervisorData,
            ILogger<Event> logger,
            IMapper mapper,
            INotify notifier,
            IEmailAttachmentSender emailAttachmentSender,
            IEventAttendancePdfService eventAttendancePdf
        ) : base(data, logger, mapper)
        {
            _data = data;
            _audienceRepo = audienceRepo;
            _apData = apData;
            _attendanceData = attendanceData;
            _supervisorData = supervisorData;
            _mapper = mapper;
            _logger = logger;
            _notifier = notifier;
            _emailAttachmentSender = emailAttachmentSender;
            _pdfGenerator = eventAttendancePdf;
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
            // 1. Obtener eventos completos desde Data
            var events = await _data.GetAllEventsWithDetailsAsync();

            // 2. Crear lista final
            var result = new List<EventDetailsDtoResponse>();

            foreach (var ev in events)
            {
                // 3. Obtener supervisores (por evento)
                var supervisors = await _supervisorData.GetSupervisorsWithUserAsync(ev.Id);

                // 4. Mapear el evento
                var dto = _mapper.Map<EventDetailsDtoResponse>(ev);

                // 5. Agregar supervisores al DTO
                dto.Supervisors = supervisors.Select(s => new EventSupervisorDtoResponse
                {
                    Id = s.Id, // viene de BaseDTO
                    EventId = s.EventId,
                    EventName = ev.Name,
                    UserId = s.UserId,
                    FullName = $"{s.User?.Person?.FirstName} {s.User?.Person?.LastName}".Trim(),
                    UserEmail = s.User?.Person?.Email
                }).ToList();

                // 6. Agregar a la respuesta
                result.Add(dto);
            }

            return result;
        }

        /// <summary>
        /// Crea un evento con accesos, audiencias y genera un código QR.
        /// </summary>
            public async Task<int> CreateEventAsync(CreateEventRequest dto)
            {
                try
                {
                    // 1️⃣ Crear el evento principal
                    var ev = _mapper.Map<Event>(dto.Event);
                    var savedEvent = await _data.SaveAsync(ev);

                    // 2️⃣ Crear Access Points
                    var createdAccessPoints = new List<AccessPoint>();
                    var links = new List<EventAccessPoint>();
              

                if (dto.AccessPoints?.Any() == true)
                {
                    createdAccessPoints = dto.AccessPoints.Select(apDto => new AccessPoint
                    {
                        Name = apDto.Name,
                        Description = apDto.Description,
                        TypeId = apDto.TypeId,
                        IsDeleted = false,
                        QrCode = null
                    }).ToList();

                    await _apData.BulkInsertAsync(createdAccessPoints);

                    links = createdAccessPoints.Select(ap =>
                    {
                        string payload = $"AP:{ap.Id}|EVENT:{savedEvent.Id}|DATE:{DateTime.UtcNow:O}";

                        return new EventAccessPoint
                        {
                            EventId = savedEvent.Id,
                            AccessPointId = ap.Id,
                            QrCodeKey = QrCodeHelper.ToPngBase64(payload)
                        };
                    }).ToList();

                    await _data.BulkInsertEventAccessPointsAsync(links);
                }


                // 3️⃣ Crear QR GENERAL DEL EVENTO
                {
                    string firstAccessPoint = createdAccessPoints.FirstOrDefault()?.Name ?? "General";

                    string qrContent = $"EVT|{savedEvent.Id}|{savedEvent.Name}|{firstAccessPoint}";

                    savedEvent.QrCodeBase64 = GenerateQrBase64(qrContent);

                    await _data.UpdateAsync(savedEvent);
                }



                // 4️⃣ Crear Audiencias
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

                    // 5️⃣ NUEVO: Registrar jornadas (Schedules)
                    if (dto.ScheduleIds != null && dto.ScheduleIds.Any())
                    {
                        var scheduleLinks = dto.ScheduleIds.Select(sid => new EventSchedule
                        {
                            EventId = savedEvent.Id,
                            ScheduleId = sid
                        }).ToList();

                        await _data.BulkInsertEventSchedulesAsync(scheduleLinks);
                    }

                    // 6️⃣ Guardar Supervisores
                    if (dto.SupervisorUserIds?.Any() == true)
                    {
                        var supervisors = dto.SupervisorUserIds.Select(uid => new EventSupervisor
                        {
                            EventId = savedEvent.Id,
                            UserId = uid
                        });

                        await _supervisorData.BulkInsertAsync(supervisors);
                    }


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

                var now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

                if (ev.Schedules != null && ev.Schedules.Any())
                {
                        foreach (var sch in ev.Schedules)
                        {
                            var today = now.DayOfWeek.ToString();
                            var scheduleDays = sch.Days?.Split(',').Select(d => d.Trim()) ?? Enumerable.Empty<string>();

                        if (!scheduleDays.Contains(today, StringComparer.OrdinalIgnoreCase))
                                continue;

                            if (now.TimeOfDay >= sch.StartTime && now.TimeOfDay <= sch.EndTime)
                            {
                                if (ev.StatusId != 8)
                                {
                                    ev.StatusId = 8;
                                    ev.UpdateAt = DateTime.Now;
                                    await _data.UpdateAsync(ev);

                                    _logger.LogInformation($"Evento {ev.Id} marcado como 'En curso'.");
                                }
                                
                                return; // ya no revisamos más
                            }
                        }
                    }
                if (ev.StatusId == 8)
                {
                    ev.StatusId = 1; // Por ejemplo:  No iniciado
                    ev.UpdateAt = DateTime.Now;
                    await _data.UpdateAsync(ev);

                    _logger.LogInformation($"Evento {ev.Id} marcado como 'No en curso'.");
                }
            }


            /// <summary>
            /// Actualiza un evento existente con sus relaciones.
            /// </summary>
            public async Task<int> UpdateEventAsync(EventDtoRequest dto)
            {
                try
                {
                    var existingEvent = await _data.GetEventWithDetailsAsync(dto.Id);
                    if (existingEvent == null)
                        throw new EntityNotFoundException($"No se encontró el evento con Id {dto.Id}");

                    // 1️⃣ Datos principales
                    existingEvent.Name = dto.Name;
                    existingEvent.Description = dto.Description;
                    existingEvent.EventStart = dto.EventStart;
                    existingEvent.EventEnd = dto.EventEnd;
                    existingEvent.EventTypeId = dto.EventTypeId;
                    existingEvent.StatusId = dto.StatusId;
                    existingEvent.IsPublic = dto.Ispublic;
                    existingEvent.UpdateAt = DateTime.Now;

                    // 2️⃣ Actualizar Access Points
                    await _data.DeleteEventAccessPointsByEventIdAsync(existingEvent.Id);

                    if (dto.AccessPoints != null && dto.AccessPoints.Any())
                    {
                        var newLinks = dto.AccessPoints.Select(apId => new EventAccessPoint
                        {
                            EventId = existingEvent.Id,
                            AccessPointId = apId
                        }).ToList();

                        await _data.BulkInsertEventAccessPointsAsync(newLinks);
                    }

                    // 3️⃣ Actualizar Audiencias
                    await _audienceRepo.DeleteByEventIdAsync(existingEvent.Id);

                    await CreateEventAudiencesAsync(
                        existingEvent.Id,
                        dto.ProfileIds,
                        dto.OrganizationalUnitIds,
                        dto.InternalDivisionIds
                    );

                    // 4️⃣ NUEVO: Actualizar jornadas del evento
                    await _data.DeleteEventSchedulesByEventIdAsync(existingEvent.Id);

                    if (dto.ScheduleIds != null && dto.ScheduleIds.Any())
                    {
                        var scheduleLinks = dto.ScheduleIds.Select(sid => new EventSchedule
                        {
                            EventId = existingEvent.Id,
                            ScheduleId = sid
                        }).ToList();

                        await _data.BulkInsertEventSchedulesAsync(scheduleLinks);
                    }

                    // 5️⃣ Guardar actualización
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
        /// Genera QR en Base64 usando QrCodeHelper (mismo que AccessPoints)
        /// </summary>
        private string GenerateQrBase64(string content)
        {
            return QrCodeHelper.ToPngBase64(content);
        }

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


        // ============================================================
        //  NUEVO MÉTODO: FILTRAR EVENTOS POR ESTADO, TIPO Y PÚBLICO
        // ============================================================
        public async Task<IEnumerable<EventDtoResponse>> FilterAsync(EventFilterDto filters)
        {
            var query = _data.GetQueryable()
                .Where(e => !e.IsDeleted);

            if (filters.StatusId.HasValue)
                query = query.Where(e => e.StatusId == filters.StatusId.Value);

            if (filters.EventTypeId.HasValue)
                query = query.Where(e => e.EventTypeId == filters.EventTypeId.Value);

            if (filters.IsPublic.HasValue)
                query = query.Where(e => e.IsPublic == filters.IsPublic.Value);

            var list = await _data.ToListAsync(query);
            return list.Select(e => _mapper.Map<EventDtoResponse>(e));
        }

        /// <summary>
        /// Para obtener el conteo de eventos por tipo de evento
        /// </summary>
        /// <returns></returns>
        public async Task<List<EventTypeCountDtoResponse>> GetEventTypeCountsAsync()
        {
            return await _data.GetEventTypeCountsAsync();
        }

        public async Task<List<EventAttendanceTopDtoResponse>> GetTopEventsByTypeAsync(int eventTypeId, int top = 5)
        {
            return await _data.GetTopEventsByTypeAsync(eventTypeId, top);
        }

        public async Task<IEnumerable<EventSupervisor>> FinalizeEventAndNotifyAsync(int eventId)
        {
            var ev = await _data.GetByIdAsync(eventId)
                ?? throw new Exception("Evento no encontrado");

            var eventEntity = await _data.GetByIdAsync(eventId);
            if (eventEntity == null)
                throw new Exception("Evento no encontrado.");

            var supervisors = await _data.GetSupervisorsByEventIdAsync(eventId);
            var attendees = await _attendanceData.GetByEventIdAsync(eventId);

            var pdfBytes = await _pdfGenerator.GenerateEventAttendancePdfAsync(
                ev,
                attendees,
                supervisors
            );

            var subject = $"Reporte de Asistencia - {eventEntity.Name}";
            var pdfName = $"Asistencia_{eventEntity.Name}_{DateTime.UtcNow:yyyyMMdd_HHmm}.pdf";
            var body = $@"
            Hola,

            Adjunto encontrarás el reporte de asistencia del evento: {eventEntity.Name}.

            Fecha de inicio: {eventEntity.EventStart:dd/MM/yyyy HH:mm}
            Fecha de finalización: {eventEntity.EventEnd:dd/MM/yyyy HH:mm}

            Cordial saludo,
            Sistema de Carnetización Digital
            ";


            foreach (var sup in supervisors)
            {
                if (string.IsNullOrWhiteSpace(sup.User.Person.Email))
                {
                    // Registrar y saltar
                    _logger.LogWarning(
                     "[FinalizeEvent] Supervisor {FullName} (UserId {UserId}) no tiene correo. Se omitió notificación.",
                     $"{sup.User.Person.FirstName} {sup.User.Person.LastName}",
                     sup.UserId
                 );

                    continue;
                }

                try
                {
                    await _emailAttachmentSender.SendEmailWithAttachmentAsync(
                        sup.User.Person.Email,
                        subject,
                        body,
                        pdfBytes,
                        pdfName
                    );
                }
                catch (Exception ex)
                {
                    // Error controlado
                    _logger.LogError(ex,
                        "[FinalizeEvent] Error enviando correo al supervisor {Email}.",
                        sup.User.Person.Email);

                    // Opcional: devolver mensaje al front
                    throw new Exception($"No se pudo enviar correo al supervisor {sup.User.UserName} ({sup.User.Person.Email}). Verifique la configuración.");
                }
            }


            return supervisors;
        }
    }
}

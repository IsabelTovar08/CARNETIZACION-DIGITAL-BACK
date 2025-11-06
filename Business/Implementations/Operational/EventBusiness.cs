using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        /// Crea un evento con accesos, audiencias y genera un código QR corto con datos útiles.
        /// </summary>
        public async Task<int> CreateEventAsync(CreateEventRequest dto)
        {
            // 1️⃣ Mapear el evento desde DTO
            var ev = _mapper.Map<Event>(dto.Event);

            // Primero guardamos el evento (para tener su ID disponible para el QR)
            var savedEvent = await _data.SaveAsync(ev);

            // 2️⃣ Crear puntos de acceso si vienen en el DTO
            var createdAccessPoints = new List<AccessPoint>();
            if (dto.AccessPoints?.Any() == true)
            {
                createdAccessPoints = dto.AccessPoints
                    .Select(apDto => _mapper.Map<AccessPoint>(apDto))
                    .ToList();

                await _apData.BulkInsertAsync(createdAccessPoints);

                var links = createdAccessPoints.Select(ap => new EventAccessPoint
                {
                    EventId = savedEvent.Id,
                    AccessPointId = ap.Id
                }).ToList();

                await _data.BulkInsertEventAccessPointsAsync(links);
            }

            // ✅ 3️⃣ Generar código QR corto con: ID, nombre y punto de acceso
            string firstAccessPoint = createdAccessPoints.FirstOrDefault()?.Name ?? "General";
            string qrContent = $"EVT|{savedEvent.Id}|{savedEvent.Name}|{firstAccessPoint}";

            // Generar imagen QR (pequeña) y codificarla en Base64
            using var qrGen = new QRCodeGenerator();
            using var qrData = qrGen.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrData);
            using var bitmap = qrCode.GetGraphic(6);
            using var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            savedEvent.QrCodeBase64 = Convert.ToBase64String(ms.ToArray());

            // Guardar el evento con su QR actualizado
            await _data.UpdateAsync(savedEvent);

            // 4️⃣ Crear audiencias (perfiles, unidades, divisiones)
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

        /// <summary>
        /// Obtiene los detalles completos del evento, incluyendo accesos, audiencias y QR.
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
    }
}

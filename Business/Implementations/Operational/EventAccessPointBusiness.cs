using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Business.Services.CodeGenerator;
using Data.Interfases;
using Data.Interfases.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Operational
{
    public class EventAccessPointBusiness : BaseBusiness<EventAccessPoint, EventAccessPointDtoRequest, EventAccessPointDto>, IEventAccessPointBusiness
    {
        protected readonly IEventAccessPointData _data;
        public EventAccessPointBusiness(IEventAccessPointData data, ILogger<EventAccessPoint> logger, IMapper mapper, ICodeGeneratorService<EventAccessPoint>? codeService = null) : base(data, logger, mapper, codeService)
        {
            _data = data;
        }


        /// <summary>
        /// Valida duplicados desde Business. Lanza excepción si existe duplicado.
        /// </summary>
        public async Task ValidateDuplicate(int eventId, int accessPointId)
        {
            // Validaciones de negocio
            if (eventId <= 0)
                throw new ArgumentException("El EventId no es válido.");

            if (accessPointId <= 0)
                throw new ArgumentException("El AccessPointId no es válido.");

            // Validación en data
            bool exists = await _data.ExistsDuplicateAsync(eventId, accessPointId);

            if (exists)
                throw new InvalidOperationException(
                    "Ya existe un punto de acceso asignado a este evento.");
        }


        /// <summary>
        /// Crea un EventAccessPoint con validación en Business.
        /// </summary>
        public override async Task<EventAccessPointDto> Save(EventAccessPointDtoRequest dto)
        {
            // 1. Validación en Business
            await ValidateDuplicate(dto.EventId, dto.AccessPointId);

            // 2. Mapear
            var entity = _mapper.Map<EventAccessPoint>(dto);

            // 3. Guardar
            return _mapper.Map<EventAccessPointDto>(await _data.SaveAsync(entity));
        }

        ///  <inheritdoc/>
        public async Task<EventAccessPoint?> GetByQrKey(string qrKey)
        {
            try
            {
                return await _data.GetByQrKeyAsync(qrKey);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error al consultar el Punto de Acesso del evento a través del UNIQUEKEY: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Actualiza un EventAccessPoint con validación en Business.
        /// </summary>
        public override async Task<EventAccessPointDto> Update(EventAccessPointDtoRequest dto)
        {
            // Validación
            await ValidateDuplicate(dto.EventId, dto.AccessPointId);

            var entity = _mapper.Map<EventAccessPoint>(dto);

            return _mapper.Map<EventAccessPointDto>(await _data.UpdateAsync(entity));
        }

    }
}

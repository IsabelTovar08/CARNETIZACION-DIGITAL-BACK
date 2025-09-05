using System;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Data.Interfases.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;
using Microsoft.Extensions.Logging;
using Utilities.Helpers; // ✅ usamos el helper genérico en Utilities

namespace Business.Implementations.Operational
{
    public class AccessPointBusiness : BaseBusiness<AccessPoint, AccessPointDtoRequest, AccessPointDtoResponsee>, IAccessPointBusiness
    {
        private readonly IAccessPointData _data;
        private readonly ILogger<AccessPoint> _logger;
        private readonly IMapper _mapper;

        public AccessPointBusiness(IAccessPointData data, ILogger<AccessPoint> logger, IMapper mapper)
            : base(data, logger, mapper)
        {
            _data = data;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Registra el Punto de Acceso y genera el QR (Base64 PNG) usando Utilities.
        /// </summary>
        public async Task<AccessPointDtoResponsee?> RegisterAsync(AccessPointDtoRequest dto)
        {
            if (dto == null || dto.EventId <= 0 || dto.TypeId <= 0)
            {
                _logger.LogWarning("Datos inválidos al registrar AccessPoint.");
                return null;
            }

            // 1) Guardar para obtener Id
            AccessPointDtoResponsee created = await Save(dto);
            if (created == null)
            {
                _logger.LogWarning("No se pudo crear el AccessPoint.");
                return null;
            }

            try
            {
                // 2) Construir payload del QR (ejemplo no sensible)
                string payload = $"AP:{created.Id}|EVENT:{created.EventId}|DATE:{DateTime.UtcNow:O}";

                // 3) Generar QR Base64 PNG con Utilities (genérico para cualquier string)
                created.QrCode = QrCodeHelper.ToPngBase64(payload);

                // 4) Actualizar registro con el QR
                AccessPointDtoRequest updateRequest = _mapper.Map<AccessPointDtoRequest>(created);
                await Update(updateRequest);

                _logger.LogInformation($"AccessPoint registrado con QR (Id={created.Id}).");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando QR para AccessPoint.");
                return created; // Ya quedó creado; solo falló el QR
            }
        }
    }
}

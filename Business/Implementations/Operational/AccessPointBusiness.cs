using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Data.Interfases.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;
using Microsoft.Extensions.Logging;
using Utilities.Helpers;

namespace Business.Implementations.Operational
{
    public class AccessPointBusiness : BaseBusiness<AccessPoint, AccessPointDtoRequest, AccessPointDtoResponsee>, IAccessPointBusiness
    {
        private readonly IAccessPointData _data;
        private readonly IAttendanceData _attendanceData;
        private readonly IEventData _eventData;
        private readonly ILogger<AccessPoint> _logger;
        private readonly IMapper _mapper;

        public AccessPointBusiness(
            IAccessPointData data,
            IAttendanceData attendanceData,
            IEventData eventData,
            ILogger<AccessPoint> logger,
            IMapper mapper
        ) : base(data, logger, mapper)
        {
            _data = data;
            _attendanceData = attendanceData;
            _eventData = eventData;
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

            AccessPointDtoResponsee created = await Save(dto);
            if (created == null)
            {
                _logger.LogWarning("No se pudo crear el AccessPoint.");
                return null;
            }

            try
            {
                string payload = $"AP:{created.Id}|EVENT:{created.EventId}|DATE:{DateTime.UtcNow:O}";
                created.QrCode = QrCodeHelper.ToPngBase64(payload);

                AccessPointDtoRequest updateRequest = _mapper.Map<AccessPointDtoRequest>(created);
                await Update(updateRequest);

                _logger.LogInformation($"AccessPoint registrado con QR (Id={created.Id}).");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando QR para AccessPoint.");
                return created;
            }
        }

        /// <summary>
        /// Registra asistencia escaneando un QR (evento debe estar activo).
        /// </summary>
        public async Task<AttendanceDtoResponse?> RegisterAttendanceByQrAsync(string qrCode, int personId)
        {
            if (string.IsNullOrWhiteSpace(qrCode))
                throw new ArgumentException("El código QR es obligatorio.", nameof(qrCode));

            if (personId <= 0)
                throw new ArgumentException("El identificador de la persona no es válido.", nameof(personId));

            try
            {
                // Decodificar QR (base64 → texto)
                var decoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(qrCode));
                // Formato esperado: "AP:{id}|EVENT:{id}|DATE:{fecha}"
                var parts = decoded.Split('|')
                    .Select(p => p.Split(':'))
                    .Where(p => p.Length == 2)
                    .ToDictionary(p => p[0], p => p[1]);

                if (!parts.ContainsKey("EVENT") || !int.TryParse(parts["EVENT"], out int eventId))
                    return new AttendanceDtoResponse { Success = false, Message = "QR inválido: no contiene evento." };

                var evento = await _eventData.GetByIdAsync(eventId);
                if (evento == null)
                    return new AttendanceDtoResponse { Success = false, Message = "Evento no encontrado." };

                // ✅ Validamos usando EventEnd y StatusId
                if ((evento.EventEnd.HasValue && evento.EventEnd.Value < DateTime.UtcNow) || evento.StatusId != 1)
                    return new AttendanceDtoResponse { Success = false, Message = "El evento está cerrado o inactivo." };

                // Crear asistencia
                var attendance = new AttendanceDtoRequest
                {
                    PersonId = personId,
                    TimeOfEntry = DateTime.UtcNow,
                    AccessPointOfEntry = null
                };

                var entity = _mapper.Map<Attendance>(attendance);
                await _attendanceData.SaveAsync(entity); // ✅ método correcto para insertar

                var response = _mapper.Map<AttendanceDtoResponse>(entity);
                response.Success = true;
                response.Message = "Asistencia registrada correctamente.";
                response.EventName = evento.Description ?? evento.Code; // ✅ usamos Description si existe
                return response;
            }
            catch (FormatException)
            {
                return new AttendanceDtoResponse { Success = false, Message = "El código QR no es válido." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar asistencia desde QR.");
                return new AttendanceDtoResponse { Success = false, Message = "Error interno al registrar asistencia." };
            }
        }
    }
}

using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Data.Interfases.Operational;
using Entity.Models.Organizational;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

// dotnet add <TU_PROYECTO> package QRCoder
using QRCoder;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Operational.Request;

namespace Business.Implementations.Operational
{
    public class AttendanceBusiness : BaseBusiness<Attendance, AttendanceDtoRequest, AttendanceDtoResponse>, IAttendanceBusiness
    {
        private readonly IAttendanceData _attendanceData;
        private readonly ILogger<Attendance> _logger;
        private readonly IMapper _mapper;

        public AttendanceBusiness(
            IAttendanceData data,
            ILogger<Attendance> logger,
            IMapper mapper
        ) : base(data, logger, mapper)
        {
            _attendanceData = data;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Registra asistencia (desde escaneo móvil), normaliza campos y genera el QR.
        /// </summary>
        public async Task<AttendanceDtoResponse?> RegisterAttendanceAsync(AttendanceDtoRequest dto)
        {
            if (dto == null || dto.PersonId <= 0)
            {
                _logger.LogWarning("Intento de registrar asistencia con datos inválidos (DTO nulo o PersonId <= 0).");
                return null;
            }

            try
            {
                // Si no viene hora de entrada, usar ahora.
                if (dto.TimeOfEntry == default)
                    dto.TimeOfEntry = DateTime.Now;

                // Normalizar: si llegan 0 (o negativos) convertir a NULL para no violar FK
                if (dto.AccessPointOfEntry.HasValue && dto.AccessPointOfEntry.Value <= 0)
                    dto.AccessPointOfEntry = null;

                if (dto.AccessPointOfExit.HasValue && dto.AccessPointOfExit.Value <= 0)
                    dto.AccessPointOfExit = null;

                // Si envían TimeOfExit anterior a TimeOfEntry, lo descartamos (entrada primero)
                if (dto.TimeOfExit.HasValue && dto.TimeOfExit.Value < dto.TimeOfEntry)
                    dto.TimeOfExit = null;

                // 🔹 Construir payload del QR (no sensible) y generarlo en Base64 (PNG)
                string qrPayload = BuildQrPayload(dto);

                // Guardar asistencia (usa Save del BaseBusiness)
                var created = await Save(dto);

                _logger.LogInformation($"Asistencia registrada correctamente para PersonId={dto.PersonId}, EventId={created.EventName}.");
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar asistencia en RegisterAttendanceAsync.");
                return null;
            }
        }

        /// <summary>
        /// Payload estable y fácil de validar. Evita datos sensibles.
        /// </summary>
        private static string BuildQrPayload(AttendanceDtoRequest dto)
        {
            // Se prioriza punto de entrada; si no hay, se usa salida; si no hay ninguno -> "NA"
            var apId = dto.AccessPointOfEntry ?? dto.AccessPointOfExit;
            var apText = apId.HasValue ? apId.Value.ToString() : "NA";

            // Puedes ajustar el formato si quieres JSON; esto es compacto y suficiente para validar.
            return $"PERSON:{dto.PersonId}|ACCESS:{apText}|DATE:{DateTime.UtcNow:O}";
        }

        /// <summary>
        /// Genera QR PNG (Base64). Baja el tamaño para que el Base64 no sea gigante.
        /// </summary>
        private static string GenerateQrCodeBase64(string content)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrData);

            // Tamaño moderado para acortar el Base64; ajusta si lo quieres más grande
            byte[] qrBytes = qrCode.GetGraphic(10);
            return Convert.ToBase64String(qrBytes);
        }
    }
}

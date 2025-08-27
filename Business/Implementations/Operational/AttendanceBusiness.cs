using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Data.Interfases.Operational;
using Entity.Models.Organizational;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using QRCoder;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Operational.Request;
using System.Threading;

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
        /// Escaneo QR: si NO hay asistencia abierta -> ENTRADA;
        /// si SÍ hay una abierta -> SALIDA sobre esa misma.
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
                // Normalización de AccessPoints (0 o negativos -> null)
                if (dto.AccessPointOfEntry.HasValue && dto.AccessPointOfEntry.Value <= 0)
                    dto.AccessPointOfEntry = null;
                if (dto.AccessPointOfExit.HasValue && dto.AccessPointOfExit.Value <= 0)
                    dto.AccessPointOfExit = null;

                // ¿Existe asistencia abierta?
                var open = await _attendanceData.GetOpenAttendanceAsync(dto.PersonId, CancellationToken.None);

                if (open == null)
                {
                    // ➜ ENTRADA (crear)
                    if (dto.TimeOfEntry == default)
                        dto.TimeOfEntry = DateTime.Now;

                    dto.TimeOfExit = null; // no se registra aún
                    var created = await Save(dto);
                    _logger.LogInformation($"Entrada registrada. PersonId={dto.PersonId}, AttendanceId={created.Id}");
                    return created;
                }
                else
                {
                    // ➜ SALIDA (actualizar por data layer)
                    var now = DateTime.Now;
                    if (now < open.TimeOfEntry) now = open.TimeOfEntry;

                    int? apOut = dto.AccessPointOfExit.HasValue && dto.AccessPointOfExit.Value > 0
                        ? dto.AccessPointOfExit
                        : (open.AccessPointOfExit ?? open.AccessPointOfEntry);

                    var updatedEntity = await _attendanceData.UpdateExitAsync(open.Id, now, apOut, CancellationToken.None);
                    var updated = _mapper.Map<AttendanceDtoResponse>(updatedEntity);

                    _logger.LogInformation($"Salida registrada. PersonId={dto.PersonId}, AttendanceId={updated.Id}");
                    return updated;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar asistencia (entrada/salida).");
                return null;
            }
        }

        /// <summary>
        /// REGISTRA SOLO LA ENTRADA (falla si ya existe una abierta).
        /// </summary>
        public async Task<AttendanceDtoResponse> RegisterEntryAsync(
            AttendanceDtoRequestSpecific dto,
            CancellationToken ct = default)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.PersonId <= 0) throw new ArgumentException("El PersonId debe ser mayor que 0.", nameof(dto.PersonId));

            var open = await _attendanceData.GetOpenAttendanceAsync(dto.PersonId, ct);
            if (open != null)
                throw new InvalidOperationException("Ya existe una ENTRADA abierta. Debe registrarse la SALIDA antes de crear otra ENTRADA.");

            var time = dto.Time == default ? DateTime.Now : dto.Time;
            int? apIn = (dto.AccessPoint.HasValue && dto.AccessPoint.Value > 0) ? dto.AccessPoint : null;

            var req = new AttendanceDtoRequest
            {
                PersonId = dto.PersonId,
                TimeOfEntry = time,
                TimeOfExit = null,
                AccessPointOfEntry = apIn,
                AccessPointOfExit = null
            };

            var created = await Save(req); // creación
            _logger.LogInformation("Entrada registrada (endpoint específico). PersonId={PersonId}, AttendanceId={AttendanceId}", dto.PersonId, created.Id);
            return created!;
        }

        /// <summary>
        /// REGISTRA SOLO LA SALIDA (falla si no hay entrada abierta).
        /// </summary>
        public async Task<AttendanceDtoResponse> RegisterExitAsync(
            AttendanceDtoRequestSpecific dto,
            CancellationToken ct = default)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.PersonId <= 0) throw new ArgumentException("El PersonId debe ser mayor que 0.", nameof(dto.PersonId));

            var open = await _attendanceData.GetOpenAttendanceAsync(dto.PersonId, ct);
            if (open == null)
                throw new InvalidOperationException("No existe una ENTRADA abierta para registrar la SALIDA.");

            var time = dto.Time == default ? DateTime.Now : dto.Time;
            if (time < open.TimeOfEntry) time = open.TimeOfEntry;

            int? apOut = (dto.AccessPoint.HasValue && dto.AccessPoint.Value > 0)
                ? dto.AccessPoint
                : (open.AccessPointOfExit ?? open.AccessPointOfEntry);

            // Actualizar por data layer (evita conflicto de tracking y crea UPDATE real)
            var updatedEntity = await _attendanceData.UpdateExitAsync(open.Id, time, apOut, ct);
            var updated = _mapper.Map<AttendanceDtoResponse>(updatedEntity);

            _logger.LogInformation("Salida registrada (endpoint específico). PersonId={PersonId}, AttendanceId={AttendanceId}", dto.PersonId, updated.Id);
            return updated!;
        }

        private static string BuildQrPayload(AttendanceDtoRequest dto)
        {
            var apId = dto.AccessPointOfEntry ?? dto.AccessPointOfExit;
            var apText = apId.HasValue ? apId.Value.ToString() : "NA";
            return $"PERSON:{dto.PersonId}|ACCESS:{apText}|DATE:{DateTime.UtcNow:O}";
        }

        private static string GenerateQrCodeBase64(string content)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrData);
            byte[] qrBytes = qrCode.GetGraphic(10);
            return Convert.ToBase64String(qrBytes);
        }
    }
}

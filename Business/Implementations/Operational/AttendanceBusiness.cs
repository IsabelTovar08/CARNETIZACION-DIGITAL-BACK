using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Data.Interfases.Operational;
using Data.Interfases.Security;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Reports;
using Entity.Models.Organizational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Helpers;

namespace Business.Implementations.Operational
{
    public class AttendanceBusiness : BaseBusiness<Attendance, AttendanceDtoRequest, AttendanceDtoResponse>, IAttendanceBusiness
    {
        private readonly IAttendanceData _attendanceData;
        private readonly IEventData _eventData;
        private readonly IPersonData _personData;
        private readonly ILogger<Attendance> _logger;
        private readonly IMapper _mapper;

        public AttendanceBusiness(
            IAttendanceData attendanceData,
            IEventData eventData,
            IPersonData personData,
            ILogger<Attendance> logger,
            IMapper mapper
        ) : base(attendanceData, logger, mapper)
        {
            _attendanceData = attendanceData;
            _eventData = eventData;
            _personData = personData;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// ✅ Registra asistencia automáticamente al leer un código QR.
        /// QR esperado: EVT|EventId|EventName|AccessPointName
        /// </summary>
        public async Task<AttendanceDtoResponse> RegisterAttendanceByQrAsync(string qrContent, int personId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(qrContent))
                    return new AttendanceDtoResponse { Success = false, Message = "QR vacío o inválido." };

                var parts = qrContent.Split('|');
                if (parts.Length < 4)
                    return new AttendanceDtoResponse { Success = false, Message = "Formato de QR inválido." };

                // Descomponer datos del QR
                int eventId = int.Parse(parts[1]);
                string eventName = parts[2].Trim();
                string accessPointName = parts[3].Trim();

                // Buscar evento
                var ev = await _eventData.GetQueryable()
                    .Include(e => e.EventAccessPoints)
                        .ThenInclude(eap => eap.AccessPoint)
                    .FirstOrDefaultAsync(e => e.Id == eventId && !e.IsDeleted);

                if (ev == null)
                    return new AttendanceDtoResponse { Success = false, Message = "Evento no encontrado." };

                // Buscar persona
                var person = await _personData.GetQueryable().FirstOrDefaultAsync(p => p.Id == personId);
                if (person == null)
                    return new AttendanceDtoResponse { Success = false, Message = "Persona no encontrada." };

                // Buscar punto de acceso
                var accessPoint = ev.EventAccessPoints
                    .Select(eap => eap.AccessPoint)
                    .FirstOrDefault(ap => ap.Name == accessPointName)
                    ?? ev.EventAccessPoints.FirstOrDefault()?.AccessPoint;

                if (accessPoint == null)
                    return new AttendanceDtoResponse { Success = false, Message = "Punto de acceso no encontrado." };

                // Verificar si ya hay asistencia abierta
                var open = await _attendanceData.GetOpenAttendanceAsync(personId);
                if (open != null)
                {
                    return new AttendanceDtoResponse
                    {
                        Success = false,
                        Message = "La persona ya tiene una asistencia abierta."
                    };
                }

                // Crear registro de asistencia
                var attendance = new Attendance
                {
                    PersonId = person.Id,
                    TimeOfEntry = DateTime.UtcNow,
                    AccessPointOfEntry = accessPoint.Id,
                    QrCode = qrContent
                };

                var saved = await _attendanceData.SaveAsync(attendance);

                return new AttendanceDtoResponse
                {
                    Success = true,
                    Message = $"Asistencia registrada en '{ev.Name}' - Punto: '{accessPoint.Name}'.",
                    EventName = ev.Name,
                    TimeOfEntry = saved.TimeOfEntry,
                    TimeOfEntryStr = saved.TimeOfEntry.ToString("dd/MM/yyyy HH:mm"),
                    PersonName = $"{person.FirstName} {person.LastName}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar asistencia mediante QR.");
                return new AttendanceDtoResponse
                {
                    Success = false,
                    Message = "Error interno al registrar asistencia."
                };
            }
        }

        /// <summary>
        /// Registra asistencia general (sin entrada/salida específica).
        /// </summary>
        public async Task<AttendanceDtoResponse?> RegisterAttendanceAsync(AttendanceDtoRequest dto)
        {
            try
            {
                var entity = _mapper.Map<Attendance>(dto);
                var saved = await _attendanceData.SaveAsync(entity);
                var response = _mapper.Map<AttendanceDtoResponse>(saved);

                response.Success = true;
                response.Message = "Asistencia registrada correctamente.";
                response.TimeOfEntryStr = response.TimeOfEntry.ToString("dd/MM/yyyy HH:mm");

                if (response.TimeOfExit.HasValue)
                    response.TimeOfExitStr = response.TimeOfExit.Value.ToString("dd/MM/yyyy HH:mm");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando asistencia.");
                return new AttendanceDtoResponse
                {
                    Success = false,
                    Message = "Error al registrar asistencia."
                };
            }
        }

        public async Task<AttendanceDtoResponse> RegisterEntryAsync(AttendanceDtoRequestSpecific dto, CancellationToken ct = default)
        {
            try
            {
                var open = await _attendanceData.GetOpenAttendanceAsync(dto.PersonId, ct);
                if (open != null)
                {
                    return new AttendanceDtoResponse
                    {
                        Success = false,
                        Message = "La persona ya tiene una entrada activa."
                    };
                }

                var entity = new Attendance
                {
                    PersonId = dto.PersonId,
                    TimeOfEntry = DateTime.UtcNow,
                    AccessPointOfEntry = dto.AccessPointId
                };

                var saved = await _attendanceData.SaveAsync(entity);
                var response = _mapper.Map<AttendanceDtoResponse>(saved);

                response.Success = true;
                response.Message = "Entrada registrada correctamente.";
                response.TimeOfEntryStr = response.TimeOfEntry.ToString("dd/MM/yyyy HH:mm");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando entrada.");
                return new AttendanceDtoResponse
                {
                    Success = false,
                    Message = "Error al registrar entrada."
                };
            }
        }

        public async Task<AttendanceDtoResponse> RegisterExitAsync(AttendanceDtoRequestSpecific dto, CancellationToken ct = default)
        {
            try
            {
                var open = await _attendanceData.GetOpenAttendanceAsync(dto.PersonId, ct);
                if (open == null)
                {
                    return new AttendanceDtoResponse
                    {
                        Success = false,
                        Message = "No se encontró una asistencia abierta para la persona."
                    };
                }

                var updated = await _attendanceData.UpdateExitAsync(open.Id, DateTime.UtcNow, dto.AccessPointId, ct);
                var response = _mapper.Map<AttendanceDtoResponse>(updated);

                response.Success = true;
                response.Message = "Salida registrada correctamente.";
                response.TimeOfEntryStr = response.TimeOfEntry.ToString("dd/MM/yyyy HH:mm");

                if (response.TimeOfExit.HasValue)
                    response.TimeOfExitStr = response.TimeOfExit.Value.ToString("dd/MM/yyyy HH:mm");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando salida.");
                return new AttendanceDtoResponse
                {
                    Success = false,
                    Message = "Error al registrar salida."
                };
            }
        }

        public async Task<(IList<AttendanceDtoResponse> Items, int Total)> SearchAsync(
            int? personId, int? eventId, DateTime? fromUtc, DateTime? toUtc,
            string? sortBy, string? sortDir, int page, int pageSize,
            CancellationToken ct = default)
        {
            var (entities, total) = await _attendanceData.QueryAsync(
                personId, eventId, fromUtc, toUtc, sortBy, sortDir, page, pageSize, ct);

            var list = entities.Select(e => _mapper.Map<AttendanceDtoResponse>(e)).ToList();

            foreach (var it in list)
            {
                it.TimeOfEntryStr = it.TimeOfEntry.ToString("dd/MM/yyyy HH:mm");
                if (it.TimeOfExit.HasValue)
                    it.TimeOfExitStr = it.TimeOfExit.Value.ToString("dd/MM/yyyy HH:mm");

                var entity = entities.FirstOrDefault(e => e.Id == it.Id);
                if (entity != null)
                {
                    var evFromEntry = entity.AccessPointEntry?.EventAccessPoints
                        .Select(eap => eap.Event?.Name)
                        .FirstOrDefault();

                    var evFromExit = entity.AccessPointExit?.EventAccessPoints
                        .Select(eap => eap.Event?.Name)
                        .FirstOrDefault();

                    it.EventName = evFromEntry ?? evFromExit;
                }
            }

            return (list, total);
        }

        public async Task<byte[]> ExportToPdfAsync(IEnumerable<AttendanceDtoResponse> data, CancellationToken ct = default)
        {
            return await Task.Run(() =>
                ExportHelper.ExportAttendancesToPdf(data, "Reporte de Asistencias"), ct);
        }

        public async Task<byte[]> ExportToExcelAsync(IEnumerable<AttendanceDtoResponse> data, CancellationToken ct = default)
        {
            return await Task.Run(() =>
                ExportHelper.ExportAttendancesToExcel(data, "Asistencias"), ct);
        }
    }
}

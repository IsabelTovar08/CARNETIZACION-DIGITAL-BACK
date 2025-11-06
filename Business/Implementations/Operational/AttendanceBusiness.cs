using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Data.Interfases.Operational;
using Entity.Models.Organizational;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Reports;
using Utilities.Helpers; // ✅ cambio aquí

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

        /// <summary>
        /// Registra la entrada de una persona a un evento.
        /// </summary>
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

        /// <summary>
        /// Registra la salida de una persona de un evento.
        /// </summary>
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

        /// <summary>
        /// Búsqueda de asistencias con filtros y paginación.
        /// </summary>
        public async Task<(IList<AttendanceDtoResponse> Items, int Total)> SearchAsync(
    int? personId, int? eventId, DateTime? fromUtc, DateTime? toUtc,
    string? sortBy, string? sortDir, int page, int pageSize,
    CancellationToken ct = default)
        {
            var (entities, total) = await _attendanceData.QueryAsync(
                personId, eventId, fromUtc, toUtc, sortBy, sortDir, page, pageSize, ct);

            var list = entities.Select(e => _mapper.Map<AttendanceDtoResponse>(e)).ToList();

            // completar strings amigables y EventName
            foreach (var it in list)
            {
                it.TimeOfEntryStr = it.TimeOfEntry.ToString("dd/MM/yyyy HH:mm");
                if (it.TimeOfExit.HasValue)
                    it.TimeOfExitStr = it.TimeOfExit.Value.ToString("dd/MM/yyyy HH:mm");

                // Buscar nombre de evento a través de AccessPoints
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

        // NUEVOS MÉTODOS DE EXPORTACIÓN (usando ExportHelper)
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

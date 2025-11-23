using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Auth;
using Business.Interfaces.Notifications;
using Business.Interfaces.Operational;
using Business.Interfaces.Security;
using Data.Implementations.Operational;
using Data.Interfases.Operational;
using Data.Interfases.Security;
using DocumentFormat.OpenXml.EMMA;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Reports;
using Entity.Enums.Specifics;
using Entity.Models.Organizational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utilities.Helpers;

namespace Business.Implementations.Operational
{
    public class AttendanceBusiness : BaseBusiness<Attendance, AttendanceDtoRequest, AttendanceDtoResponse>, IAttendanceBusiness
    {
        private readonly IAttendanceData _attendanceData;
        private readonly IEventData _eventData;
        private readonly IPersonData _personData;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<Attendance> _logger;
        private readonly IMapper _mapper;
        private readonly IUserBusiness _userBusiness;
        private readonly IEventAccessPointBusiness _eventAccessPointBusiness;
        private readonly INotificationBusiness _notificationBusiness;
        private readonly IAttendanceNotifier _attendanceNotifier;
        public AttendanceBusiness(
            IAttendanceData attendanceData,
            IEventData eventData,
            IPersonData personData,
            ILogger<Attendance> logger,
            IMapper mapper,
            ICurrentUser currentUser,
            IUserBusiness userBusiness,
            IEventAccessPointBusiness eventAccessPointBusiness,
            INotificationBusiness notificationBusiness,
            IAttendanceNotifier attendanceNotifier
        ) : base(attendanceData, logger, mapper)
        {
            _attendanceData = attendanceData;
            _eventData = eventData;
            _personData = personData;
            _logger = logger;
            _mapper = mapper;
            _currentUser = currentUser;
            _userBusiness = userBusiness;
            _eventAccessPointBusiness = eventAccessPointBusiness;
            _notificationBusiness = notificationBusiness;
            _attendanceNotifier = attendanceNotifier;
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
                //var open = await _attendanceData.GetOpenAttendanceAsync(personId, eventAccessPointEntryId);
                //if (open != null)
                //{
                //    return new AttendanceDtoResponse
                //    {
                //        Success = false,
                //        Message = "La persona ya tiene una asistencia abierta."
                //    };
                //}

                // Crear registro de asistencia
                var attendance = new Attendance
                {
                    PersonId = person.Id,
                    TimeOfEntry = DateTime.UtcNow,
                    EventAccessPointEntryId = accessPoint.Id,
                    //EventId = ev.Id,
                    //QrCode = qrContent
                };

                var saved = await _attendanceData.SaveAsync(attendance);

                // ✅ Recargar entidad con relaciones
                var reloaded = await _attendanceData.GetAllAsync();
                var full = reloaded.FirstOrDefault(a => a.Id == saved.Id);

                var response = _mapper.Map<AttendanceDtoResponse>(full);
                response.Success = true;
                response.Message = $"Asistencia registrada en '{ev.Name}' - Punto: '{accessPoint.Name}'.";
                return response;
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

        public async Task<AttendanceDtoResponse?> RegisterAttendanceAsync(AttendanceDtoRequest dto)
        {
            try
            {
                UserDTO? user = await _userBusiness.GetById(_currentUser.UserId);
                dto.PersonId = user?.PersonId ?? dto.PersonId;
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
        /// Registra una entrada validando si ya existe una asistencia abierta para la persona.
        /// </summary>
        public async Task<AttendanceDtoResponse> RegisterEntryAsync(AttendanceDtoRequestSpecific dto)
        {
            try
            {
                // Obtener persona
                UserDTO? user = await _userBusiness.GetById(_currentUser.UserId);
                dto.PersonId = user?.PersonId ?? dto.PersonId;

                // Obtener EventAccessPointId 
                int eventAccessPointId = await ResolveEventAccessPointIdAsync(dto.QrCodeKey);

                // Validar si la persona ya tiene asistencia abierta en este mismo punto
                var open = await _attendanceData.GetOpenAttendanceAsync(dto.PersonId, eventAccessPointId);
                if (open != null)
                {
                    return new AttendanceDtoResponse
                    {
                        Success = false,
                        Message = "Ya existe una entrada registrada. ¿Desea realizar la salida?"
                    };
                }

                // Crear DTO para guardar
                var dtoRequest = new AttendanceDtoRequest
                {
                    PersonId = dto.PersonId,
                    TimeOfEntry = DateTime.UtcNow,
                    EventAccessPointEntryId = eventAccessPointId
                };

                // Guardar entrada
                var saved = await Save(dtoRequest);

                // Recargar para retorno
                var reloaded = await GetById(saved.Id);
                var response = _mapper.Map<AttendanceDtoResponse>(reloaded);

                await _notificationBusiness.SendTemplateAsync(
                    NotificationTemplateType.AttendanceEntry,
                    response.TimeOfEntry,      // args[0]
                    reloaded.EventName,           // args[1]
                    reloaded.AccessPointOfEntryName      // args[2]
                );


                await _attendanceNotifier.NotifyExitAsync(response);

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
        /// Registra salida utilizando el método Update normal.
        /// </summary>
        public async Task<AttendanceDtoResponse> RegisterExitAsync(AttendanceDtoRequestSpecific dto)
        {
            try
            {
                // Obtener persona desde JWT
                UserDTO? user = await _userBusiness.GetById(_currentUser.UserId);
                dto.PersonId = user?.PersonId ?? dto.PersonId;

                // Obtener EventAccessPointId desde el QR
                int eventAccessPointId = await ResolveEventAccessPointIdAsync(dto.QrCodeKey);

                // Buscar asistencia abierta
                Attendance? open = await _attendanceData.GetOpenAttendanceAsync(dto.PersonId, eventAccessPointId);
                if (open == null)
                {
                    return new AttendanceDtoResponse
                    {
                        Success = false,
                        Message = "No se encontró una asistencia abierta para registrar salida."
                    };
                }

                var updateDto = _mapper.Map<AttendanceDtoRequest>(open);

                updateDto.EventAccessPointExitId = eventAccessPointId;
                updateDto.TimeOfExit = DateTime.UtcNow;

                // Update
                var updated = await Update(updateDto);

                // Recargar DTO completo
                var reloaded = await GetById(updated.Id);
                var response = _mapper.Map<AttendanceDtoResponse>(reloaded);

                await _notificationBusiness.SendTemplateAsync(
                    NotificationTemplateType.AttendanceExit,
                    response.TimeOfEntry,      // args[0]
                    reloaded.EventName,           // args[1]
                    reloaded.AccessPointOfEntryName      // args[2]
                );

                await _attendanceNotifier.NotifyExitAsync(response);

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
            // 1️⃣ Obtener entidades desde Data
            var (entities, total) = await _attendanceData.QueryAsync(
                personId, eventId, fromUtc, toUtc, sortBy, sortDir, page, pageSize, ct);

            // 2️⃣ Mapear DTOs
            var list = entities
                .Select(e => _mapper.Map<AttendanceDtoResponse>(e))
                .ToList();


            foreach (var dto in list)
            {
                var entity = entities.First(e => e.Id == dto.Id);

                dto.AccessPointOfEntryName = entity.EventAccessPointEntry?.AccessPoint?.Name ?? "";
                dto.AccessPointOfExitName = entity.EventAccessPointExit?.AccessPoint?.Name;

                dto.EventName = entity.EventAccessPointEntry?.Event?.Name
                              ?? entity.EventAccessPointExit?.Event?.Name;

                dto.EventId = entity.EventAccessPointEntry?.EventId;

                dto.HasMoreAttendances = await _attendanceData.PersonHasMoreAttendancesAsync(
                    entity.PersonId,
                    dto.EventId!.Value,
                    entity.Id
                );
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

        /// <summary>
        /// Resuelve el EventAccessPointId usando el qrKey del QR.
        /// </summary>
        private async Task<int> ResolveEventAccessPointIdAsync(string qrKey)
        {
            var ap = await _eventAccessPointBusiness.GetByQrKey(qrKey);

            if (ap == null)
                throw new Exception("QR inválido o no registrado.");

            return ap.Id;
        }


        public async Task<IList<AttendanceDtoResponse>> GetAllByPersonEventAsync(int personId, int eventId)
        {
            try
            {
                return await _attendanceData.GetAllByPersonEventAsync(personId, eventId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener todas las asistencias de la persona para el evento.");
            }
        }
    }
}

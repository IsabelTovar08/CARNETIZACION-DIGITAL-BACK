using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Data.Interfases.Operational;
using Entity.DTOs.Operational;
using Entity.Models.Organizational;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Business.Implementations.Operational
{
    public class AttendanceBusiness : BaseBusiness<Attendance, AttendanceDto, AttendanceDto>, IAttendanceBusiness
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
        /// Registra asistencia a través de un QR escaneado.
        /// </summary>
        public async Task<AttendanceDto?> RegisterAttendanceAsync(AttendanceDto dto)
        {
            if (dto == null || dto.PersonId <= 0)
            {
                _logger.LogWarning("Intento de registrar asistencia con datos inválidos.");
                return null;
            }

            try
            {
                // Si no viene hora de entrada → poner hora actual
                if (dto.TimeOfEntry == default)
                    dto.TimeOfEntry = DateTime.Now;

                // Salida puede quedar en null (aún no salió)
                var created = await Save(dto);

                _logger.LogInformation(
                    $" Asistencia registrada correctamente para la persona con ID {dto.PersonId}."
                );
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " Error al registrar asistencia en RegisterAttendanceAsync.");
                return null;
            }
        }
    }
}

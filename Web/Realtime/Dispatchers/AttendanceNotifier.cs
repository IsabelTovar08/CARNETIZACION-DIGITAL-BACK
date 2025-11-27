using Business.Implementations.Operational;
using Entity.DTOs.Operational.Response;
using Microsoft.AspNetCore.SignalR;
using Web.Realtime.Hubs;

namespace Web.Realtime.Dispatchers
{
    public class AttendanceNotifier : IAttendanceNotifier
    {
        private readonly IHubContext<AttendanceHub> _hub;

        public AttendanceNotifier(IHubContext<AttendanceHub> hub)
        {
            _hub = hub;
        }

        public async Task NotifyEntryAsync(AttendanceDtoResponse dto)
        {
            await _hub.Clients.All.SendAsync("AttendanceEntry", new
            {
                attendanceId = dto.Id,
                personId = dto.PersonId,
                eventId = dto.EventId,
                eventName = dto.EventName,
                accessPoint = dto.AccessPointOfEntryName,
                time = dto.TimeOfEntryStr
            });
        }

        public async Task NotifyExitAsync(AttendanceDtoResponse dto)
        {
            await _hub.Clients.All.SendAsync("AttendanceExit", new
            {
                attendanceId = dto.Id,
                personId = dto.PersonId,
                eventId = dto.EventId,
                eventName = dto.EventName,
                accessPoint = dto.AccessPointOfEntryName,
                time = dto.TimeOfExitStr
            });
        }
    }
}

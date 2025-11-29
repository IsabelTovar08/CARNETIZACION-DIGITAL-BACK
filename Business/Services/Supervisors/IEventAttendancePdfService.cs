using Entity.Models.Operational;
using Entity.Models.Organizational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Supervisors
{
    public interface IEventAttendancePdfService
    {
        /// <summary>
        /// Genera un PDF con la lista de asistentes de un evento.
        /// </summary>
        Task<byte[]> GenerateEventAttendancePdfAsync(
        Event ev,
        List<Attendance> attendees,
        List<EventSupervisor> supervisors
    );
    }
}

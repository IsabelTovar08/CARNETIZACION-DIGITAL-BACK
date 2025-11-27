using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Operational.Response;

namespace Business.Implementations.Operational
{
    public interface IAttendanceNotifier
    {
        Task NotifyEntryAsync(AttendanceDtoResponse dto);
        Task NotifyExitAsync(AttendanceDtoResponse dto);
    }
}

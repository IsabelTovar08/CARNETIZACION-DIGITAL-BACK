using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Organizational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Operational
{
    public class AttendanceData : CrudBase<Attendance>, IAttendanceData
    {
        public AttendanceData(ApplicationDbContext context, ILogger<Attendance> logger)
            : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Attendance>> GetAllAsync()
        {
            return await _context.Set<Attendance>().Include(x => x.AccessPointEntry).Include(x => x.AccessPointExit).ThenInclude(x => x.Event)
                .Include(x => x.Person)
                .ToListAsync();
        }
    }
}

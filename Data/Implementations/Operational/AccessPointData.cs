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
    public class AccessPointData : BaseData<AccessPoint>, IAccessPointData
    {
        public AccessPointData(ApplicationDbContext context, ILogger<AccessPoint> logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<AccessPoint>> GetAllAsync()
        {
            return await _context.Set<AccessPoint>().Include(x => x.AccessPointType).Include(x => x.Event).ToListAsync();
        }
    }
}

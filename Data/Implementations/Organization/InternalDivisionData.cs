using Data.Classes.Base;
using Entity.Context;
using Entity.Models.Organizational.Structure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations.Organization
{
    public class InternalDivisionData : BaseData<InternalDivision>
    {
        public InternalDivisionData(ApplicationDbContext context, ILogger<InternalDivision> logger)
            :base(context, logger) { }
        public Task<int> CountByOrgUnitAsync(int organizationUnitId)
        {
            return _context.Set<InternalDivision>()
                .AsNoTracking()
                .CountAsync(d => d.OrganizationalUnitId == organizationUnitId);
        }
    }
}

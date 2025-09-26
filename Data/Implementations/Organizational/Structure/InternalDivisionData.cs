using Data.Classes.Base;
using Data.Interfases.Organizational.Structure;
using Entity.Context;
using Entity.Models.Organizational.Structure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations.Organizational.Structure
{
    public class InternalDivisionData : BaseData<InternalDivision>, IInternalDivisionData
    {
        public InternalDivisionData(ApplicationDbContext context, ILogger<InternalDivision> logger) 
            :base(context, logger) { }
        public Task<int> CountByOrgUnitAsync(int organizationUnitId)
        {
            return _context.Set<InternalDivision>()
                .AsNoTracking()
                .CountAsync(d => d.OrganizationalUnitId == organizationUnitId);
        }

        public override async Task<IEnumerable<InternalDivision>> GetAllAsync()
        {
            return await _context.Set<InternalDivision>()
                .Include(x => x.OrganizationalUnit).
                Include(x => x.AreaCategory).ToListAsync();
        }

        public Task<List<InternalDivision>> ListByOrgUnitAsync(int organizationalUnitId) =>
            _context.Set<InternalDivision>()
                .Where(d => d.OrganizationalUnitId == organizationalUnitId)
                .Include(d => d.OrganizationalUnit)
                .Include(d => d.AreaCategory)
                .OrderBy(d => d.Description) 
                .AsNoTracking()
                .ToListAsync();
    }
}

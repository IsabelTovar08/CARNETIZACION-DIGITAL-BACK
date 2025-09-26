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
    public class OrganizationalUnitBranchData: BaseData<OrganizationalUnitBranch>, IOrganizationalUnitBranchData
    {
        public OrganizationalUnitBranchData(ApplicationDbContext context, ILogger<OrganizationalUnitBranch> logger)
            : base(context, logger) { }

        public Task<int> CountBranchesByOrgUnitAsync(int organizationalUnitId)
        {
            return _context.Set<OrganizationalUnitBranch>()
                .AsNoTracking()
                .CountAsync(x => x.OrganizationUnitId == organizationalUnitId);
        }


    }
}

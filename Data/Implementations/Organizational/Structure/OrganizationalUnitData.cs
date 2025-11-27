using Data.Classes.Base;
using Data.Interfases;
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
    public class OrganizationalUnitData : BaseData<OrganizationalUnit>, IOrganizationalUnitData
    {
        public OrganizationalUnitData(ApplicationDbContext context, ILogger<OrganizationalUnit> logger)  : base(context, logger)
        {
        }

        public async Task<OrganizationalUnit?> GetFullByIdAsync(int id)
        {
            return await _context.OrganizationalUnits
                .Include(x => x.InternalDivissions)
                .Include(x => x.OrganizationalUnitBranches)
                      .ThenInclude(link => link.Branch)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Organizational.Structure;
using Entity.Context;
using Entity.Models.Organizational.Structure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Organizational.Structure
{
    public class OrganizationData : BaseData<Organization>, IOrganizationData
    {
        public OrganizationData(ApplicationDbContext context, ILogger<Organization> logger) : base(context, logger)
        {
        }
        public override async Task<IEnumerable<Organization>> GetAllAsync()
        {
            return await _context.Set<Organization>().Include(x => x.OrganizaionType.Organization).ToListAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Organizational.Structure;
using DocumentFormat.OpenXml.Office2010.Excel;
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

        public async Task<Organization?> GetOrganizationByPersonId(int userId)
        {
            return await _context.Users
        .Where(u => u.Id == userId)
        .Include(u => u.Organization)
        .Select(u => u.Organization)
        .FirstOrDefaultAsync();
        }
        public async Task<bool> UpdateOrganizationAsync(Organization organization)
        {
            _context.Organizations.Update(organization);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}

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
    public class ContactOrganizationData : BaseData<ContactOrganization>, IContactOrganizationData
    {
        public ContactOrganizationData(ApplicationDbContext context, ILogger<ContactOrganization> logger) : base(context, logger)
        {
        }

     
        public override async Task<ContactOrganization?> GetByIdAsync(int id)
        {
            return await _context.ContactOrganizations
                .Include(c => c.Person)
                    .ThenInclude(p => p.DocumentType)
                .Include(c => c.Person)
                    .ThenInclude(p => p.BloodType)
                .Include(c => c.Person)
                    .ThenInclude(p => p.City)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public override async Task<IEnumerable<ContactOrganization>> GetAllAsync()
        {
            return await _context.ContactOrganizations
                .Include(c => c.Person)
                    .ThenInclude(p => p.DocumentType.Name)
                .Include(c => c.Person)
                    .ThenInclude(p => p.BloodType.Name)
                .Include(c => c.Person)
                    .ThenInclude(p => p.City.Name)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }


    }
}

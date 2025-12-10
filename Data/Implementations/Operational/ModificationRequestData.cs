using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Operational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Operational
{
    public class ModificationRequestData : BaseData<ModificationRequest>, IModificationRequestData
    {
        public ModificationRequestData(ApplicationDbContext context, ILogger<ModificationRequest> logger) : base(context, logger)
        {
        }


        public override async Task<IEnumerable<ModificationRequest>> GetAllAsync()
        {
            return await _context.Set<ModificationRequest>()
                .Include(x => x.User)
                    .ThenInclude(u => u.Person)
                .OrderByDescending(x => x.CreateAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ModificationRequest>> GetByUserIdAsync(int userId)
        {
            return await _context.modificationRequests
                .Where(r => r.UserId == userId && !r.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}

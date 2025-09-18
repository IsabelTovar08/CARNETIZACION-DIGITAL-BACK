using Data.Classes.Base;
using Data.Interfases.Security;
using Entity.Context;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Classes.Specifics
{
    public class ModuleData : BaseData<Module>, IModuleData
    {
        public ModuleData(ApplicationDbContext context, ILogger<Module> logger) : base(context, logger)
        {

        }

        public async override Task<IEnumerable<Module>> GetActiveAsync()
        {
            return await _context.Set<Module>().Where(x => !x.IsDeleted).Include(x => x.Forms).ToListAsync();
        }

        public async Task<List<Module>> GetModulesWithFormsByAllowedFormsAsync(List<int> allowedFormIds)
        {
            return await _context.Set<Module>()
                .AsNoTracking()
                .AsSplitQuery()
                .Where(m => !m.IsDeleted)
                .Include(m => m.Forms.Where(f => allowedFormIds.Contains(f.Id) && !f.IsDeleted))
                .ToListAsync();
        }
    }
}
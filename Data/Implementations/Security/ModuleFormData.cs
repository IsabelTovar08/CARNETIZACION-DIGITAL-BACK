using Data.Classes.Base;

using Entity.Context;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Classes.Specifics
{
    public class ModuleFormData : BaseData<ModuleForm>
    {
        private ApplicationDbContext context;
        private ILogger<ModuleFormData> _logger;
        public ModuleFormData(ApplicationDbContext context, ILogger<ModuleForm> logger) : base(context, logger)
        {
            this.context = context;
        }

        public override async Task<IEnumerable<ModuleForm>> GetAllAsync()
        {
            try
            {
                return await context.Set<ModuleForm>()
                    .Include(mf => mf.Module)
                    .Include(mf => mf.Form)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ModuleForms");
                throw;
            }
        }

        public override async Task<ModuleForm> GetByIdAsync(int id)
        {
            try
            {
                return await context.Set<ModuleForm>()
                    .Include(mf => mf.Module)
                    .Include(mf => mf.Form)
                    .FirstOrDefaultAsync(fm => fm.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ModuleForms");
                throw;
            }
        }

        

    }
}


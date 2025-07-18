using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Data.Classes.Base;
using Entity.Context;

namespace Data.Classes.Specifics
{
    public class RolFormPermissionData : BaseData<RolFormPermission>
    {
        private ApplicationDbContext context;
        private ILogger<RolFormPermissionData> _logger;
        public RolFormPermissionData(ApplicationDbContext context, ILogger<RolFormPermission> logger) : base(context, logger)
        {
            this.context = context;
        }

        public override async Task<IEnumerable<RolFormPermission>> GetAllAsync()
        {
            try
            {
                return await context.Set<RolFormPermission>()
                    .Include(rfp => rfp.Rol)
                    .Include(rfp => rfp.Form)
                    .Include(rfp => rfp.Permission)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving RolFormPermissions");
                throw;
            }
        }

        public override async Task<RolFormPermission> GetByIdAsync(int id)
        {
            try
            {
                return await context.Set<RolFormPermission>()
                    .Include(rfp => rfp.Rol)
                    .Include(rfp => rfp.Form)
                    .Include(rfp => rfp.Permission)
                    .FirstOrDefaultAsync(rfp => rfp.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving RolFormPermissions");
                throw;
            }
        }



    }
}


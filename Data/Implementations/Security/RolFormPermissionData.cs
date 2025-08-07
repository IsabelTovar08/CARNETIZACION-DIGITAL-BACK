using Data.Classes.Base;
using Data.Interfases;
using Entity.Context;
using Entity.DTOs.ModelSecurity;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Classes.Specifics
{
    public class RolFormPermissionData : BaseData<RolFormPermission>, IRolFormPermissionData
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

        public async Task<List<RolFormPermissionsCompletedDto>> GetAllRolFormPermissionsAsync()
        {
            var result = await context.Set<RolFormPermission>()
                .Include(rfp => rfp.Rol)
                .Include(rfp => rfp.Form)
                .Include(rfp => rfp.Permission)
                .ToListAsync();

            var agrupado = result
                .GroupBy(rfp => new
                {
                    rfp.RolId,
                    RolName = rfp.Rol.Name,
                    rfp.FormId,
                    FormName = rfp.Form.Name
                })
                .Select(g => new RolFormPermissionsCompletedDto
                {
                    RolId = g.Key.RolId,
                    RolName = g.Key.RolName,
                    FormId = g.Key.FormId,
                    FormName = g.Key.FormName,
                    Permissions = g
                        .Select(x => new PermissionDto
                        {
                            Id = x.Permission.Id,
                            Name = x.Permission.Name,
                            Description = x.Permission.Description
                        })
                        .DistinctBy(p => p.Id)
                        .ToList()
                })
                .ToList();

            return agrupado;
        }




    }
}


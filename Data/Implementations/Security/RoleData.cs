
using Data.Classes.Base;
using Data.Interfases.Security;
using Entity;
using Entity.Context;
using Entity.DTOs.ModelSecurity.Request;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Classes.Specifics
{
    public class RoleData : BaseData<Role>, IRoleData
    {
        public RoleData(ApplicationDbContext context, ILogger<Role> logger) : base(context, logger)
        {

        }

        public async Task<bool> UserIsSuperAdminAsync(List<string> roleNames)
        {
            return await _context.Set<Role>()
                .AsNoTracking()
                .AnyAsync(r => roleNames.Contains(r.Name) && r.HasAllPermissions);
        }



    }
}
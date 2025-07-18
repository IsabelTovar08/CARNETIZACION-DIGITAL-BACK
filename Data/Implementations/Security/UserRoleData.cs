using Data.Classes.Base;
using Entity.Context;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Classes.Specifics
{
    public class UserRoleData : BaseData<UserRoles>
    {
        private ApplicationDbContext context;
        private ILogger<UserRoleData> _logger;
        public UserRoleData(ApplicationDbContext context, ILogger<UserRoles> logger) : base(context, logger)
        {
            this.context = context;
        }

        public override async Task<IEnumerable<UserRoles>> GetAllAsync()
        {
            try
            {
                return await context.Set<UserRoles>()
                    .Include(ur => ur.User)
                    .Include(ur => ur.Rol)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving UserRols");
                throw;
            }
        }

        public override async Task<UserRoles> GetByIdAsync(int id)
        {
            try
            {
                return await context.Set<UserRoles>()
                    .Include(ur => ur.User)
                    .Include(ur => ur.Rol)
                    .FirstOrDefaultAsync(ur => ur.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving UserRols");
                throw;
            }
        }

        public async Task<List<string>> GetRolesByUserIdAsync(int userId)
        {
            try
            {
                return await context.Set<UserRoles>()
                    .Include(ur => ur.Rol)
                    .Where(ur => ur.User.Id == userId)
                    .Select(ur => ur.Rol.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving roles for user with ID {userId}");
                throw;
            }
        }


    }
}


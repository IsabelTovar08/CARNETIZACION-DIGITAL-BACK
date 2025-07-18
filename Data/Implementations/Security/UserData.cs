using Data.Classes.Base;
using Entity.Context;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Classes.Specifics
{
    public class UserData : BaseData<User>
    {
        public UserData(ApplicationDbContext context, ILogger<User> logger) : base(context, logger)
        {
        }

        public async override Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Set<User>().
                Include(u => u.Person).
                ToListAsync();
        }

        public async Task<List<string>> GetUserRolesByIdAsync(int userId)
        {
            var user = await _context.Set<User>()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.UserRoles.Select(ur => ur.Rol.Name).ToList() ?? new List<string>();
        }


        public async Task<User?> FindByEmail(string email)
        {
            return await _context.Set<User>().Where(u => !u.IsDeleted).FirstOrDefaultAsync(u => u.Email == email);
        }

    }
}

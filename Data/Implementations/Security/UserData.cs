using System.Security.Cryptography;
using Data.Classes.Base;
using Data.Interfases;
using Data.Interfases.Security;
using Entity.Context;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Utilities.Helper.EncryptedPassword;

namespace Data.Classes.Specifics
{
    public class UserData : BaseData<User>, IUserData
    {
        public UserData(ApplicationDbContext context, ILogger<User> logger) : base(context, logger)
        {
        }

        public async override Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Set<User>()
                .Include(u => u.Person)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Rol)
                .ToListAsync();
        }


        public async Task<List<string>> GetUserRolesByIdAsync(int userId)
        {
            var user = await _context.Set<User>()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.UserRoles.Select(ur => ur.Rol.Name).ToList() ?? new List<string>();
        }
        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _context.Set<User>()
                .Include(u => u.Person)
                 .FirstOrDefaultAsync(u =>
                        !u.IsDeleted &&
                        (u.Person.Email == email || u.UserName == email)
                    );

            if (user == null)
                return null;

            bool isValid = VerifyPassword(password, user.Password);
            return isValid ? user : null;
        }

        public async Task<User?> FindByEmail(string email)
        {
            return await _context.Set<User>().Where(u => !u.IsDeleted)
                .Include(u => u.Person)
               .FirstOrDefaultAsync(u => u.Person.Email == email);
        }

        //Email (coincide con la validacion de las credenciales)
        public async Task<User?> FindByLoginIdentifierAsync(string identifier)
        {
            return await _context.Set<User>().Where(u => !u.IsDeleted)
                .Include(u => u.Person)
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u =>
                    u.UserName == identifier || (u.Person != null && u.Person.Email == identifier));
        }

        public async Task<User?> ChangePassword(string email)
        {
            return await _context.Set<User>().Where(u => !u.IsDeleted)
                .Include(u => u.Person)
               .FirstOrDefaultAsync(u => u.Person.Email == email);
        }

        public async Task<string?> RequestPasswordResetAsync(string email)
        {
            var user = await _context.Set<User>().FirstOrDefaultAsync(u => u.UserName == email ||  u.Person.Email == email);
            if (user == null) return null;

            // Generar token seguro
            var tokenBytes = RandomNumberGenerator.GetBytes(32);
            var token = Convert.ToBase64String(tokenBytes)
                                .Replace("+", "-")
                                .Replace("/", "")
                                .Replace("=", "");

            user.ResetCode = token;
            user.ResetCodeExpiration = DateTime.UtcNow.AddHours(1);

            _context.Update(user);
            await _context.SaveChangesAsync();

            return token;
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _context.Set<User>().FirstOrDefaultAsync(u => u.UserName == email ||  u.Person.Email == email);
            if (user == null) return false;

            // Validar token
            if (user.ResetCode != token || user.ResetCodeExpiration < DateTime.UtcNow)
                return false;

            // Hashear nueva contraseña
            user.Password = EncryptPassword(newPassword);

            // Limpiar token
            user.ResetCode = null;
            user.ResetCodeExpiration = null;

            _context.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

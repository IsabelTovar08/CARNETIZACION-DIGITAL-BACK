using Data.Classes.Base;
using Data.Interfases;
using Data.Interfases.Security;
using Entity.Context;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using Utilities.Helper;
using Utilities.Helpers;
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

        public async Task<User?> GetByIdForMeAsync(int userId, bool includeProfile)
        {
            var query = _context.Users
                .AsNoTrackingWithIdentityResolution() // <- permite fix-up sin tracking
                .Where(u => u.Id == userId && !u.IsDeleted)
                // Roles / Permisos
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Rol)
                        .ThenInclude(r => r.RolFormPermissions)
                            .ThenInclude(rp => rp.Permission)
                .AsQueryable();

            if (includeProfile)
            {
                query = query
                    // Person + City desde el root (NO volver a Person desde PDP)
                    .Include(u => u.Person)
                        .ThenInclude(p => p.City)

                    // Solo el perfil actual y sus dependencias HACIA ADELANTE
                    .Include(u => u.Person.PersonDivisionProfile.Where(pdp => pdp.IsCurrentlySelected))
                        .ThenInclude(pdp => pdp.Profile)
                    .Include(u => u.Person.PersonDivisionProfile.Where(pdp => pdp.IsCurrentlySelected))
                        .ThenInclude(pdp => pdp.InternalDivision); // o Division, según tu modelo
                                                                   // 👈 NO hagas: .ThenInclude(pdp => pdp.Person) ni .ThenInclude(...Person.City)
            }

            return await query.FirstOrDefaultAsync();
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

        public async Task<User?> VerificationPassword(string token, string password)
        {
            var userId = Verificate_Password.GetUserIdFromToken(token);

            if (string.IsNullOrEmpty(userId))
                return null;

            var user = await _context.Set<User>()

                 .FirstOrDefaultAsync(u => !u.IsDeleted && u.Id.ToString() == userId);

            if (user == null)
                return null;

            bool isValid = EncryptedPassword.VerifyPassword(password, user.Password);
            return isValid ? user : null;
        }

        public async Task<User?> GetByIdWithPersonAsync(int userId)
        {
            return await _context.Set<User>()
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}

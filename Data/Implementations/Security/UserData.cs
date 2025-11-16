using System.Security.Cryptography;
using Data.Classes.Base;
using Data.Interfases;
using Data.Interfases.Security;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.Context;
using Entity.Models;
using Entity.Models.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public async override Task<User?> GetByIdAsync(int id)
        {
            return await _context.Set<User>()
                .Include(u => u.Person)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<string>> GetUserRolesByIdAsync(int userId)
        {
            var user = await _context.Set<User>()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.UserRoles.Select(ur => ur.Rol.Name).ToList() ?? new List<string>();
        }

        /// <summary>
        /// Obtiene un usuario por Id con sus carnets:
        /// - El seleccionado (IsCurrentlySelected)
        /// - Los demás
        /// </summary>
        public async Task<User?> GetByIdForMeAsync(int userId)
        {
            return await _context.Users
                .AsNoTrackingWithIdentityResolution()
                .Where(u => u.Id == userId && !u.IsDeleted)

                // Roles
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Rol)
                        .ThenInclude(r => r.RolFormPermissions)
                            .ThenInclude(rp => rp.Permission)

                // Person + City
                .Include(u => u.Person)
                    .ThenInclude(p => p.City)

                // Todos los IssuedCards, pero SIN profile completo
                .Include(u => u.Person.IssuedCard)

                .FirstOrDefaultAsync();
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

        public async Task<Person?> GetPersonByUserIdAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId && !u.IsDeleted)
                .Include(u => u.Person)
                .Select(u => u.Person)
                .FirstOrDefaultAsync();
        }


        /// <summary>
        /// Obtiene todos los usuarios que tienen asignado un rol específico (por nombre del rol).
        /// </summary>
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return Enumerable.Empty<User>();

            return await _context.Users
                .Where(u => !u.IsDeleted &&
                            u.UserRoles.Any(ur => ur.Rol.Name.ToLower() == roleName.ToLower()))
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Rol)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}

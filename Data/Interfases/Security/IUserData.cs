using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models;

namespace Data.Interfases.Security
{
    public interface IUserData : ICrudBase<User>
    {
        Task<User?> ValidateUserAsync(string email, string password);
        Task<User?> FindByEmail(string email);

        Task<User?> FindByLoginIdentifierAsync(string identifier);

        Task<string?> RequestPasswordResetAsync(string email);

        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    }
}

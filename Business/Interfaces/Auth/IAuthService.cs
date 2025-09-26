using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.DTOs.Auth;
using Entity.Models;

namespace Business.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<User> LoginAsync(LoginRequest loginRequest);
        Task ChangePasswordAsync(int userId, string currentPassword, string newPassword);

        Task<string?> RequestPasswordResetAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    }
}

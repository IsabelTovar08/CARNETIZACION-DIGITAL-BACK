using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Auth
{
    public interface IUserVerificationService
    {
        Task GenerateAndSendAsync(User user);
        Task<bool> VerifyAsync(int userId, string code);
        Task<bool> ResendAsync(int userId);
    }
}

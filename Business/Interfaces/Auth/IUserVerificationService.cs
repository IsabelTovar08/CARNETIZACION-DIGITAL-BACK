using Entity.DTOs.Auth;
using Entity.DTOs.ModelSecurity.Response;
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
        Task<VerifyResult> VerifyAsync(int userId, string code);
        Task<VerifyResult> ResendAsync(int userId);
    }
}

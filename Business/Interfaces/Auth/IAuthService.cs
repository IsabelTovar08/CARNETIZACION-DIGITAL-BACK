using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Auth;
using Entity.Models;

namespace Business.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<User> LoginAsync(LoginRequest loginRequest);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Auth
{
    public interface IJwtService
    {
        (string token, string jti) GenerateToken(string userId, string username);
    }
}

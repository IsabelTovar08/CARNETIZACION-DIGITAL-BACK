using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Auth
{
    /// <summary>
    /// Resultado del flujo de login.
    /// Si RequiresTwoFactor = true, se envía UserId para verify-code.
    /// Si RequiresTwoFactor = false, Tokens contiene el JWT.
    /// </summary>
    public class LoginResultDto
    {
        public bool RequiresTwoFactor { get; set; }
        public int? UserId { get; set; }

        public AuthTokens? Tokens { get; set; }
        public bool IsFirstLogin { get; set; }
    }
}

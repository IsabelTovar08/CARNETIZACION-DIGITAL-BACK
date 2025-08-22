using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Auth
{
    public class ResetPasswordRequest
    {
        public string Token { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.ModelSecurity.Request
{
    public class VerifyPasswordDtoRequest
    {
        public string Password { get; set; } = null!;
    }
}

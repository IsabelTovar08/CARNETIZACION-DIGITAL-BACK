    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Auth
{
    public class VerifyResult
    {
        public bool Success { get; set; }
        public string Error { get; set; } = string.Empty;

        public static VerifyResult Ok() => new() { Success = true };
        public static VerifyResult Fail(string e) => new() { Success = false, Error = e };

    }

}

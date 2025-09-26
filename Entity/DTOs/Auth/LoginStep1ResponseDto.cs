using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Auth
{
    public sealed class LoginStep1ResponseDto
    {
        //public bool RequiresCode { get; set; }
        public bool isFirtsLogin   { get; set; }
        public string Message { get; set; }
        //public string userName { get; set; }
        public int UserId { get; set; }
    }
}

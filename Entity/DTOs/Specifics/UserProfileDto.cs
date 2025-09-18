using Entity.DTOs.Base;
using Entity.DTOs.ModelSecurity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics
{
    public class UserProfileDto 
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? SecondLastName { get; set; }

        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
    }
}

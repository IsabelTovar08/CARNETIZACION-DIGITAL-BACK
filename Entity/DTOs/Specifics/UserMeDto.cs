using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Organizational.Assigment.Response;

namespace Entity.DTOs.Specifics
{
    public class UserMeDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }

        public List<RolDto> Roles { get; set; } = new();
        public List<PermissionDto> Permissions { get; set; } = new();

        public IssuedCardDto? CurrentProfile { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Organizational.Assigment.Response;
using Entity.DTOs.Specifics.Cards;

namespace Entity.DTOs.Specifics
{
    public class UserMeDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public int PersonId { get; set; }

        public List<RolDto> Roles { get; set; } = new();
        public List<PermissionDto> Permissions { get; set; } = new();
        /// <summary>Perfil completo del carnet seleccionado.</summary>
        public CardUserData? CurrentProfile { get; set; }
        /// <summary>Lista de todos los demás carnets completos.</summary>
        public List<CardUserData> OtherCards { get; set; } = new();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entity.Models.Base;
using Entity.Models.ModelSecurity;
using Entity.Models.Organizational.Structure;

namespace Entity.Models
{
    public class User : BaseModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public List<UserRoles> UserRoles { get; set; }
        public DateTime DateCreated { get; set; }

        //Refresh Token
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Para recuperación
        public string? ResetCode { get; set; }
        public DateTime? ResetCodeExpiration { get; set; }

        //Verificar si el usuario si esta activo
        public bool Active { get; set; } = false;

        //Codigo temporal
        public string? TempCodeHash { get; set; }
        // Fecha de creacion del codigo temporal
        public DateTimeOffset? TempCodeCreatedAt { get; set; }
        // Fecha de expiracion del codigo temporal
        public DateTimeOffset? TempCodeExpiresAt { get; set; }
        public int TempCodeAttempts { get; set; }

        //Este atributo lo que hace es guardar la fecha de los intentos de codigo
        public DateTimeOffset? TempCodeConsumedAt { get; set; }
        public DateTimeOffset? TempCodeResendBlockedUntil { get; set; }

        //public int OrganizationId { get; set; }
        //public Organization Organization { get; set; } = default!;
    }
}

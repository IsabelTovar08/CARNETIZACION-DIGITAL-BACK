using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entity.Models.Base;
using Entity.Models.ModelSecurity;

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
    }
}

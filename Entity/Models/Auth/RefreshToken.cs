using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Base;

namespace Entity.Models.Auth
{
    public class RefreshToken: BaseModel
    {
        public string TokenHash { get; set; } = default!;
        public string JwtId { get; set; } = default!;

        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public DateTime? Revoked { get; set; }

        public string? ReplacedByTokenHash { get; set; }

        // Helpers
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsRevoked => Revoked.HasValue;

    }
}

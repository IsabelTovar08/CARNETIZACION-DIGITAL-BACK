using Entity.Models.Base;

namespace Entity.Models
{
    public class UserRoles : BaseModel
    {
        public int UserId { get; set; }
        public int RolId { get; set; }

        public User User { get; set; }
        public Role Rol { get; set; }

    }
}

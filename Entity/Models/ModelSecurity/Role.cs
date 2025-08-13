using Entity.Models.Base;

namespace Entity.Models
{
    public class Role : GenericModel
    {
        public string? Description { get; set; }


        public List<UserRoles> UserRoles { get; set; } = new();
        public List<RolFormPermission> RolFormPermissions { get; set; }

    }
}

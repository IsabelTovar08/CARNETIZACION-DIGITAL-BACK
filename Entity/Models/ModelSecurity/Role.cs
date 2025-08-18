using Entity.Models.Base;

namespace Entity.Models
{
    public class Role : GenericModel
    {
        public string? Description { get; set; }

        // # Rol sin restricciones (SuperAdmin)
        public bool HasAllPermissions { get; set; } = false;
        public List<UserRoles> UserRoles { get; set; } = new();
        public List<RolFormPermission> RolFormPermissions { get; set; }

    }
}

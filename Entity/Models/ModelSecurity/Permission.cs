using Entity.Models.Base;

namespace Entity.Models
{
    public class Permission : GenericModel
    {
        public string? Description { get; set; }

        public List<RolFormPermission> RolFormPermissions { get; set; }
    }
}

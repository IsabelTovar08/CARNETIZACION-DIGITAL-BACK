using Entity.Models.Base;

namespace Entity.Models
{
    public class RolFormPermission : GenericModel
    {
        public int RolId { get; set; }
        public int FormId { get; set; }
        public int PermissionId { get; set; }
        public Role Rol { get; set; }
        public Form Form { get; set; }
        public Permission Permission { get; set; }

    }
}

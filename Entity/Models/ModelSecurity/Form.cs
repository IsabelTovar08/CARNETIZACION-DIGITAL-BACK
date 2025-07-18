using Entity.Models.Base;

namespace Entity.Models
{
    public class Form : GenericModel
    { 
        public string? Description { get; set; }
        public string Url { get; set; }

        public List<RolFormPermission> RolFormPermissions { get; set; } 

        public List<ModuleForm> ModuleForm { get; set; } 
    }
}

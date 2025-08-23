using Entity.Models.Base;

namespace Entity.Models
{
    public class Form : GenericModel
    { 
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string Url { get; set; }
        public int ModuleId { get; set; }


        public List<RolFormPermission> RolFormPermissions { get; set; } 

        public Module? Module{ get; set; } 
    }
}

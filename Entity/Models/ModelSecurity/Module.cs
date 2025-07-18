using Entity.Models.Base;

namespace Entity.Models
{
    public class Module : GenericModel
    {
        public string? Description { get; set; }

        public List<ModuleForm> ModuleForm { get; set; } 
    }
}

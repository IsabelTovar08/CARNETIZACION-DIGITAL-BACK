

using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Response
{
    public class ModuleFormDto : BaseDTO
    {
        public int ModuleId { get; set; }
        public string NameModule { get; set; }
        public int FormId { get; set; }
        public string NameForm { get; set; }
    }
}

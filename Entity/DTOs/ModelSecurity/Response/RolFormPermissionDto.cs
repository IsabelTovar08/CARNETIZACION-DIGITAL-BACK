

using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Response
{
    public class RolFormPermissionDto : BaseDTO
    {
        public int RolId { get; set; }

        public string RolName { get; set; }
        public string RolDescription { get; set; }

        public int FormId { get; set; }
        public string FormName { get; set; }
        public int PermissionId { get; set; }

        public string PermissionName { get; set; }

    }
}

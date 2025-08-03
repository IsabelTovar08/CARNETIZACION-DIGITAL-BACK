

using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Response
{
    public class UserRolDto : BaseDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int RolId { get; set; }
        public string RolName { get; set; }
    }
}

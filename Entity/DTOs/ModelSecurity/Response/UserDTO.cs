

using Entity.DTOs.Base;

namespace Entity.DTOs.ModelSecurity.Response
{
    public class UserDTO : BaseDTO
    {
        public string UserName { get; set; }
        public string EmailPerson { get; set; }
        public string NamePerson { get; set; }
        public int PersonId { get; set; }
        public bool Active { get; set; }

        public List<RolDto>? Roles { get; set; }

    }
}

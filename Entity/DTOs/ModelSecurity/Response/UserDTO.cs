

using Entity.DTOs.Base;

namespace Entity.DTOs
{
    public class UserDTO : BaseDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string NamePerson { get; set; }
        public int PersonId { get; set; }

    }
}

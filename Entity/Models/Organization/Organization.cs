using Entity.Models.Base;

namespace Entity.Models.Organization
{
    public class Organization : GenericModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Login { get; set; }
    }
}

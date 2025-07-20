using Entity.Models.Base;
using Entity.Models.Others;

namespace Entity.Models.Organizational
{
    public class Organization : GenericModel
    {
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public int TypeId { get; set; }

        public CustomType OrganizaionType { get; set; }
    }
}

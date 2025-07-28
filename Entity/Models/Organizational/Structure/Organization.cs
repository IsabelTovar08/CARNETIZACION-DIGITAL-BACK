using Entity.Models.Base;
using Entity.Models.Parameter;

namespace Entity.Models.Organizational.Structure
{
    public class Organization : GenericModel
    {
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public int TypeId { get; set; }

        public CustomType OrganizaionType { get; set; }
    }
}

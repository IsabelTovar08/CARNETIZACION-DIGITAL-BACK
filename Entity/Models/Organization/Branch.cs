using Entity.Models.Base;
using Entity.Models.Notifications;

namespace Entity.Models.Organization
{
    public class Branch : GenericModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}

using Entity.Models.Base;
using Entity.Models.Notifications;
using Entity.Models.Organizational.Location;

namespace Entity.Models.Organizational.Structure
{
    public class Branch : GenericModel
    {
        public string? Location { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public int OrganizationId { get; set; }


        public Organization Organization { get; set; }
        public City City { get; set; }

        public List<OrganizationalUnit>? OrganizationalUnits { get; set; }
        public List<InternalDivision>? InternalDivisions { get; set; }
    }
}

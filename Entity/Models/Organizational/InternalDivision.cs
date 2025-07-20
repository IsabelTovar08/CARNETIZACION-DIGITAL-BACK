using Entity.Models.Base;

namespace Entity.Models.Organizational
{
    public class InternalDivision : GenericModel
    {
        public int OrganizationalUnitId { get; set; }
        public OrganizationalUnit OrganizationalUnit { get; set; }
    }
}

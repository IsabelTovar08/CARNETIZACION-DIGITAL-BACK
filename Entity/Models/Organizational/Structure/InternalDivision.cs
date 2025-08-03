using Entity.Models.Base;

namespace Entity.Models.Organizational.Structure
{
    public class InternalDivision : GenericModel
    {
        public string? Description { get; set; }
        public int OrganizationalUnitId { get; set; }
        public OrganizationalUnit? OrganizationalUnit { get; set; }

        public int AreaCategoryId { get; set; }
        public AreaCategory? AreaCategory { get; set; }

    }
}

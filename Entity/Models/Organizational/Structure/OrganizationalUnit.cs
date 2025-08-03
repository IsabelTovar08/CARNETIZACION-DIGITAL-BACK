using System.Collections.Generic;
using Entity.Models.Base;

namespace Entity.Models.Organizational.Structure
{
    public class OrganizationalUnit : GenericModel
    {
        public string? Description { get; set; }

        public List<OrganizationalUnitBranch>? OrganizationalUnitBranches { get; set; }
        public List<InternalDivision>? InternalDivissions { get; set; }

    }
}

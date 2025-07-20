using System.Collections.Generic;
using Entity.Models.Base;

namespace Entity.Models.Organizational
{
    public class OrganizationalUnit : GenericModel
    {
        public List<OrganizationalUnitBranch>? OrganizationalUnitBranches { get; set; }
        public List<InternalDivision>? InternalDivissions { get; set; }

    }
}

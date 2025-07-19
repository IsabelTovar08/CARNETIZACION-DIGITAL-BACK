using System.Collections.Generic;
using Entity.Models.Base;

namespace Entity.Models.Organization
{
    public class OrganizationalUnit : GenericModel
    {
        public string Name { get; set; }
        public ICollection<OrganizationalUnitBranch> OrganizationalUnitBranches { get; set; }
    }
}

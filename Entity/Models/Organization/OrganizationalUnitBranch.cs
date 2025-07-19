using System.ComponentModel.DataAnnotations;
using Entity.Models.Base;

namespace Entity.Models.Organization
{
    public class OrganizationalUnitBranch : GenericModel
    {
        // Foreign Keys
        public int BranchId { get; set; }
        public int OrganizationUnitId { get; set; }

        // Navigation Properties
        public Branch Branch { get; set; }
        public OrganizationalUnit OrganizationUnit { get; set; }
    }
}
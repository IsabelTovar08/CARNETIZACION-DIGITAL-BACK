using System.ComponentModel.DataAnnotations;
using Entity.Models.Base;

namespace Entity.Models.Organizational.Structure
{
    public class OrganizationalUnitBranch : BaseModel
    {
        public int BranchId { get; set; }
        public int OrganizationUnitId { get; set; }


        public Branch? Branch { get; set; }
        public OrganizationalUnit? OrganizationUnit { get; set; }
    }
}
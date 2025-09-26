using Entity.Models.Organizational.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfases.Organizational.Structure
{
    public interface IOrganizationalUnitBranchData : ICrudBase<OrganizationalUnitBranch>
    {
        Task<int> CountBranchesByOrgUnitAsync(int organizationalUnitId);
    }
}

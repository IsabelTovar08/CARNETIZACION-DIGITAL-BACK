using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Organizational.Structure;

namespace Data.Interfases.Organizational.Structure
{
    public interface IOrganizationData : ICrudBase<Organization>
    {
        Task<Organization?> GetOrganizationByPersonId(int userId);
        Task<bool> UpdateOrganizationAsync(Organization organization);
    }
}

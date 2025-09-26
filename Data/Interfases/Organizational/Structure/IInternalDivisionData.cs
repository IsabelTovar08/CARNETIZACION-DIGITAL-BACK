using Entity.Models.Organizational.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfases.Organizational.Structure
{
    public interface IInternalDivisionData : ICrudBase<InternalDivision>
    {
        Task<int> CountByOrgUnitAsync(int organizationUnitId);

        Task<List<InternalDivision>> ListByOrgUnitAsync(int organizationUnitId);
    }
}

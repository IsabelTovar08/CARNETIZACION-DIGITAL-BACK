using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.Operational;

namespace Data.Interfases.Operational
{
    public interface IModificationRequestData : ICrudBase<ModificationRequest>
    {
        Task<IEnumerable<ModificationRequest>> GetByUserIdAsync(int userId);

    }
}

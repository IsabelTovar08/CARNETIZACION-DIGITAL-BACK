using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfases
{
    public interface ISoftDelete
    {
        Task<bool> ToggleActiveAsync(int id);
    }
}

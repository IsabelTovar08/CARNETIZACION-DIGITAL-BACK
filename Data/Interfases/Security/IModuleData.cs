using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models;

namespace Data.Interfases.Security
{
    public interface IModuleData : ICrudBase<Module>
    {
        Task<List<Module>> GetModulesWithFormsByAllowedFormsAsync(List<int> allowedFormIds);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfases;
using Entity.Models.ModelSecurity;

namespace Data.Interfaces.Security
{
    public interface IMenuStructureData: ICrudBase<MenuStructure>
    {
        Task<List<MenuStructure>> GetMenuTreeForUserAsync(int userId);
    }
}

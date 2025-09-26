using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.ModelSecurity.Response;

namespace Business.Interfaces.Auth
{
    public interface IMenuService
    {
        Task<List<MenuStructureDto>> GetMenuAsync();
        Task<List<MenuStructureDto>> GetMenuForUserAsync(int userId);
    }
}

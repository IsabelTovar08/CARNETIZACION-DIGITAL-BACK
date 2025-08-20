using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models.ModelSecurity;

namespace Business.Interfaces.Security
{
    public interface IMenuStructureBusiness : IBaseBusiness<MenuStructure, MenuStructureRequest, MenuStructureDto>
    {
        Task<List<MenuStructureDto>> GetMenuTreeForUserAsync(int userId);
    }
}

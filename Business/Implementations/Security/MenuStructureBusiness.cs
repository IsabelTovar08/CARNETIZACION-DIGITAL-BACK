using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Security;
using Business.Interfases;
using Data.Interfaces.Security;
using Data.Interfases;
using Entity.DTOs.ModelSecurity.Request;
using Entity.DTOs.ModelSecurity.Response;
using Entity.Models.ModelSecurity;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Security
{
    public class MenuStructureBusiness : BaseBusiness<MenuStructure, MenuStructureRequest, MenuStructureDto>, IMenuStructureBusiness
    {
        public readonly IMenuStructureData _data;
        public MenuStructureBusiness(IMenuStructureData data, ILogger<MenuStructure> logger, IMapper mapper) : base(data, logger, mapper)
        {
            _data = data;
        }       


        public async Task<List<MenuStructureDto>> GetMenuTreeForUserAsync(int userId)
        {
            var list = await _data.GetMenuTreeForUserAsync(userId);
            return _mapper.Map<List<MenuStructureDto>>(list);
        }

    }
}

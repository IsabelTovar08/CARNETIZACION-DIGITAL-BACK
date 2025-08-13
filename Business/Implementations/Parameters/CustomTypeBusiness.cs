using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Parameters;
using Data.Interfases;
using Data.Interfases.Parameters;
using Entity.DTOs.Parameter.Request;
using Entity.DTOs.Parameter.Response;
using Entity.Models.Parameter;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Parameters
{
    public class CustomTypeBusiness : BaseBusiness<CustomType, CustomTypeRequest, CustomTypeDto>, ICustomTypeBusiness
    {
        private readonly ICustomTypeData _customTypeData;
        public CustomTypeBusiness(ICustomTypeData customTypeData, ILogger<CustomType> logger, IMapper mapper) : base(customTypeData, logger, mapper)
        {
            _customTypeData = customTypeData;
        }

        public async Task<IEnumerable<CustomTypeSpecific>> GetTypesByCategoryNameAsync(string categoryName)
        {
           var list = await _customTypeData.GetTypesByCategoryNameAsync(categoryName);
            return _mapper.Map<IEnumerable<CustomTypeSpecific>>(list);
            
        }
    }
}

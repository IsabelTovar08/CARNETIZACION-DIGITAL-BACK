using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.Parameter.Request;
using Entity.DTOs.Parameter.Response;
using Entity.Models.Parameter;

namespace Business.Interfaces.Parameters
{
    public interface ICustomTypeBusiness : IBaseBusiness<CustomType, CustomTypeRequest, CustomTypeDto>
    {
       Task<IEnumerable<CustomTypeSpecific>> GetTypesByCategoryNameAsync(string categoryName);

    }
}

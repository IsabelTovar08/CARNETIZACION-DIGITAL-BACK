using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.Organizational.Request.Structure;
using Entity.DTOs.Organizational.Response.Structure;
using Entity.Models.Organizational.Structure;

namespace Business.Interfaces.Organizational.Structure
{
    public interface ICategoryAreaBusiness : IBaseBusiness<AreaCategory,AreaCategoryDtoRequest,AreaCategoryDto>
    {
    }
}

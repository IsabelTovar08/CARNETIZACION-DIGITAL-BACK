using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Organizational.Structure;
using Data.Interfases;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models.Organizational.Structure;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Organizational.Structure
{
    public class AreaCategoryBusiness : BaseBusiness<AreaCategory, AreaCategoryDtoRequest, AreaCategoryDto>, ICategoryAreaBusiness
    {
        public AreaCategoryBusiness(ICrudBase<AreaCategory> data, ILogger<AreaCategory> logger ,IMapper mapper) : base(data, logger, mapper) 
        { 
        }
    }
}

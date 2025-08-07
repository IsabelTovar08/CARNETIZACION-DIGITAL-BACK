using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Data.Interfases;
using Entity.DTOs.Parameter;
using Entity.Models.Parameter;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Parameters
{
    public class CustomTypeBusiness : BaseBusiness<CustomType, CustomTypeDto, CustomTypeDto>
    {
        public CustomTypeBusiness(ICrudBase<CustomType> data, ILogger<CustomType> logger, IMapper mapper) : base(data, logger, mapper)
        {
        }
    }
}

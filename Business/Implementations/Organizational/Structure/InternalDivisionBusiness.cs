using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Organizational.Structure;
using Data.Interfases;
using Data.Interfases.Organizational.Structure;
using Entity.DTOs.Organizational.Structure.Request;
using Entity.DTOs.Organizational.Structure.Response;
using Entity.Models.Organizational.Structure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Organizational.Structure
{
    public class InternalDivisionBusiness : BaseBusiness<InternalDivision, InternalDivisionDtoRequest, InternalDivisionDto>, IInternalDivisionBusiness
    {
        public InternalDivisionBusiness(IInternalDivisionData data, ILogger<InternalDivision> logger, IMapper mapper) : base(data, logger, mapper)
        {
           
        }
    }
}

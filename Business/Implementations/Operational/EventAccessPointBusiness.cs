using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Business.Services.CodeGenerator;
using Data.Interfases;
using Data.Interfases.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Operational;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Operational
{
    public class EventAccessPointBusiness : BaseBusiness<EventAccessPoint, EventAccessPointDtoRequest, EventAccessPointDto>, IEventAccessPointBusiness
    {
        public EventAccessPointBusiness(IEventAccessPointData data, ILogger<EventAccessPoint> logger, IMapper mapper, ICodeGeneratorService<EventAccessPoint>? codeService = null) : base(data, logger, mapper, codeService)
        {
        }
    }
}

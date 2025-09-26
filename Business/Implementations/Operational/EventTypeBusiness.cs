using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Business.Interfases;
using Data.Interfases;
using Data.Interfases.Operational;
using Entity.DTOs.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Operational
{
    public class EventTypeBusiness : BaseBusiness<EventType, EventTypeDtoRequest, EventTypeDtoResponse>, IEventTypeBusiness
    {
        public readonly IEventTypeData _data;
        public EventTypeBusiness(IEventTypeData data, ILogger<EventType> logger, IMapper mapper) : base(data, logger, mapper)
        {
            _data = data;
        }
    }
}

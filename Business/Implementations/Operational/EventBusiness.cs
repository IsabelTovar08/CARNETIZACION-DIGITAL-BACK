using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Data.Interfases;
using Data.Interfases.Operational;
using Entity.DTOs.Operational;
using Entity.Models.Organizational;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Operational
{
    public class EventBusiness : BaseBusiness<Event, EventDtoResponse, EventDtoResponse>, IEventBusiness
    {
        public readonly IEventData _data;
        public EventBusiness(IEventData data, ILogger<Event> logger, IMapper mapper) : base(data, logger, mapper)
        {
            _data = data;
        }
    }
}

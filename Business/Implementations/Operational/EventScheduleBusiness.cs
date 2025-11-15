using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
using Data.Interfases;
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
    public class EventScheduleBusiness : BaseBusiness<EventSchedule, EventScheduleDtoRequest, EventScheduleDtoResponse>, IEventScheduleBusiness
    {
        public EventScheduleBusiness(ICrudBase<EventSchedule> data, ILogger<EventSchedule> logger, IMapper mapper) : base(data, logger, mapper)
        {
        }
    }

}

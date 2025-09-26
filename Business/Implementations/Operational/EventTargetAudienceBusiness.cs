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
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Operational
{
    public class EventTargetAudienceBusiness
        : BaseBusiness<EventTargetAudience, EventTargetAudienceDtoRequest, EventTargetAudienceDtoResponse>, IEventTargetAudienceBusiness
    {
        public readonly IEventTargetAudienceData _data;

        public EventTargetAudienceBusiness(IEventTargetAudienceData data, ILogger<EventTargetAudience> logger, IMapper mapper)
            : base(data, logger, mapper)
        {
            _data = data;
        }
    }
}

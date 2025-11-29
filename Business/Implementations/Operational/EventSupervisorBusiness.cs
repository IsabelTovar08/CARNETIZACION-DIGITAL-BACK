using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Operational;
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
    public class EventSupervisorBusiness : BaseBusiness<EventSupervisor, EventSupervisorDtoRequest, EventSupervisorDtoResponse>, IEventSupervisorBusiness
    {
        public EventSupervisorBusiness(IEventSupervisorData data, ILogger<EventSupervisor> logger, IMapper mapper) : base(data, logger, mapper)
        {
        }
    }
}

using Business.Interfases;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Operational
{
    public interface IEventTargetAudienceBusiness
        : IBaseBusiness<EventTargetAudience, EventTargetAudienceDtoRequest, EventTargetAudienceDtoResponse>
    {
    }
}

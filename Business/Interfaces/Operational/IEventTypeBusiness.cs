using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfases;
using Entity.DTOs.Operational;
using Entity.DTOs.Operational.Request;
using Entity.DTOs.Operational.Response;
using Entity.Models.Organizational;

namespace Business.Interfaces.Operational
{
    public interface IEventTypeBusiness : IBaseBusiness<EventType, EventTypeDtoRequest, EventTypeDtoResponse>
    {
    }
}

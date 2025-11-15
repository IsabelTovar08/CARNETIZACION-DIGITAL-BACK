using Data.Classes.Base;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Operational;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations.Operational
{
    public class EventScheduleData : BaseData<EventSchedule>, IEventScheduleData
    {
        public EventScheduleData(ApplicationDbContext context, ILogger<EventSchedule> logger) : base(context, logger)
        {
        }
    }
}
    
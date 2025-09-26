using Data.Classes.Base;
using Data.Interfases.Organizational.Structure;
using Entity.Context;
using Entity.Models.Organizational.Structure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations.Organizational.Structure
{
    public class ScheduleData : BaseData<Schedule>, IScheduleData
    {
        public ScheduleData(ApplicationDbContext context, ILogger<Schedule> logger) : base(context, logger)
        {
           
        }
    }
}

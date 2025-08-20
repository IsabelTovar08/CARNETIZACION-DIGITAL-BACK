using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Organizational;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Operational
{
    public class AccessPointData : CrudBase<AccessPoint>, IAccessPointData
    {
        public AccessPointData(ApplicationDbContext context, ILogger<AccessPoint> logger) : base(context, logger)
        {
        }
    }
}

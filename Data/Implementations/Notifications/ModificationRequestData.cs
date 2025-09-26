using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Notifications;
using Entity.Context;
using Entity.Models.Organizational.Assignment;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Notifications
{
    public class ModificationRequestData : BaseData<ModificationRequest>, IModificationRequestData
    {
        public ModificationRequestData(ApplicationDbContext context, ILogger<ModificationRequest> logger) : base(context, logger)
        {
        }
    }
}

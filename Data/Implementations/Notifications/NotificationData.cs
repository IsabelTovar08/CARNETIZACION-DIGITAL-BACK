using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Notifications;
using Data.Interfases.Operational;
using Entity.Context;
using Entity.Models.Notifications;
using Entity.Models.Organizational;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Notifications
{
    public class NotificationData : BaseData<Notification>, INotificationData
    {
        public NotificationData(ApplicationDbContext context, ILogger<Notification> logger)
            : base(context, logger)
        {
        }
    }
}


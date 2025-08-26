using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Classes.Base;
using Data.Interfases.Notifications;
using Entity.Context;
using Entity.Models.Notifications;
using Microsoft.Extensions.Logging;

namespace Data.Implementations.Notifications
{
    public class NotificationsReceivedData : BaseData<NotificationReceived>, INotificationsReceivedData
    {
        public NotificationsReceivedData(ApplicationDbContext context, ILogger<NotificationReceived> logger)
            : base(context, logger)
        {
        }
    }
}

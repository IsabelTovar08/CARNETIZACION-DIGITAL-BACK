using Business.Interfases;
using Entity.DTOs.Notifications;
using Entity.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Notifications
{
    public interface INotificationReceivedBusiness : IBaseBusiness<NotificationReceived, NotificationReceivedDto, NotificationReceivedDto>
    {
    }
}

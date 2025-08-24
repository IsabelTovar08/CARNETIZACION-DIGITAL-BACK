using Business.Classes.Base;
using Business.Interfases;
using Entity.DTOs.Notifications;
using Entity.DTOs.Operational;
using Entity.Models.Notifications;

namespace Business.Interfaces.Notifications
{
    public interface INotificationBusiness : IBaseBusiness<Notification, NotificationDto, NotificationDto>
    {
    
    }
}

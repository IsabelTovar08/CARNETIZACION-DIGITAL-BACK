using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Classes.Base;
using Business.Interfaces.Notifications;
using Business.Interfaces.Operational;
using Data.Interfases.Notifications;
using Data.Interfases.Operational;
using Entity.DTOs.Notifications;
using Entity.DTOs.Operational;
using Entity.Models.Notifications;
using Microsoft.Extensions.Logging;

namespace Business.Implementations.Operational
{
    public class NotificationsBusiness : BaseBusiness<Notification, NotificationDto, NotificationDto>, INotificationBusiness
    {
        public NotificationsBusiness(INotificationData data, ILogger<Notification> logger, IMapper mapper)
            : base(data, logger, mapper)
        {
        }

        // Aquí puedes agregar métodos de negocio específicos para Notifications si los necesitas
    }
}

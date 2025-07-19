using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Notifications
{
    internal class NotificationTypeDto : GenericBaseDto
    {
        public string Description { get; set; }
    }
}

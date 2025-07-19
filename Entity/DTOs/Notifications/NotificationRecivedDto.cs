using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Notifications
{
    internal class NotificationRecivedDto : GenericBaseDto
    {
        public Enum Status { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime ReadDate { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}

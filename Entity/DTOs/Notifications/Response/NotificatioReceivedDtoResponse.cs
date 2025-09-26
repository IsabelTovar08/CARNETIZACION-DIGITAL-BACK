using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Notifications.Response
{
    public class NotificationReceivedDtoResponse : BaseDTO
    {
        public int StatusId { get; set; }
        public int StatusName { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int NotificationId { get; set; }
        public int UserId { get; set; }
    }
}

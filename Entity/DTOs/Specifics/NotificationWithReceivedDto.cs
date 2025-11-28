using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums.Specifics;

namespace Entity.DTOs.Specifics
{
    public class NotificationWithReceivedDto
    {
        public int NotificationReceivedId { get; set; }
        public int NotificationId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string NotificationTypeName { get; set; }
        public int NotificationTypeId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public string? RedirectUrl { get; set; }

    }

}

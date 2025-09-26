using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics
{
    public class NotificationWithReceivedDto
    {
        public int NotificationId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string NotificationTypeName { get; set; }

        public int StatusId { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime? ReadDate { get; set; }
    }

}

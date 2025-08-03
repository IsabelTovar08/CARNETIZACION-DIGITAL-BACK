using Entity.DTOs.Base;
using Entity.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Notifications
{
    public class NotificationDto : BaseModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int NotificationTypeId { get; set; }


    }
}

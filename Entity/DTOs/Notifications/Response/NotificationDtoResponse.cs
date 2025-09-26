using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Notifications.Response
{
    public class NotificatioReceivedDtoResponse : BaseDTO
    {
        public int PersonId { get; set; }
        public int StatusId { get; set; }
        public int StatusName { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}

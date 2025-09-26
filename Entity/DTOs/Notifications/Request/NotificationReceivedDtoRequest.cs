using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Notifications.Request
{
    public class NotificationReceivedDtoRequest : GenericDtoRequest
    {
        [Required]
        public int StatusId { get; set; }

        [Required]
        public DateTime SendDate { get; set; }

        public DateTime? ReadDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [Required]
        public int NotificationId { get; set; }

        [Required]
        public int UserId { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}

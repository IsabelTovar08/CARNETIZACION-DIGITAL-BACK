using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.Base;
using Utilities.Enums.Specifics;

namespace Entity.DTOs.Notifications.Request
{
    public class NotificationReceivedDtoRequest : GenericDtoRequest
    {
        [Required]
        public NotificationStatus Status { get; set; }

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

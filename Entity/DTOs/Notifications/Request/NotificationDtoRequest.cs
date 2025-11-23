using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums.Specifics;

namespace Entity.DTOs.Notifications.Request
{
    public class NotificationDtoRequest : BaseDtoRequest
    {
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }
        public string? RedirectUrl { get; set; }
        public int? UserId { get; set; }


        [Required]
        public NotificationType NotificationType { get; set; }

    }
}
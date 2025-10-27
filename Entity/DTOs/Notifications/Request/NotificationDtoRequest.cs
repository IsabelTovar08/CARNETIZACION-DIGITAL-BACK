using Entity.DTOs.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [Required]
        public int NotificationTypeId { get; set; }

        public int? TargetUserId { get; set; }


    }
}
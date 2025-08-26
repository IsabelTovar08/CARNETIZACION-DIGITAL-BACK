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
        public int PersonId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Message { get; set; }

        public bool IsRead { get; set; } = false;

        public bool IsDeleted { get; set; } = false;
    }
}
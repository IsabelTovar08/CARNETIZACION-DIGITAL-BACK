using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Enums.Specifics;

namespace Entity.DTOs.Notifications.Request
{
    public class ModificationRequestDtoRequest
    {
        public int UserId { get; set; }
        public ModificationField Field { get; set; } 
        public string OldValue { get; set; } = "";
        public string NewValue { get; set; } = "";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Notifications.Request
{
    public class ModificationRequestDtoResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime RequestDate { get; set; }

        public int FieldId { get; set; } 
        public string FieldName { get; set; } = "";
        public string OldValue { get; set; } = "";
        public string NewValue { get; set; } = "";

        public string Status { get; set; } = "";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Enums.Specifics;
using Entity.Models.Base;

namespace Entity.Models.Organizational.Assignment
{
    /// <summary>
    /// Solicitud de modificación individual (un cambio por solicitud).
    /// </summary>
    public class ModificationRequest : BaseModel
    {
        public int UserId { get; set; }
        public DateTime RequestDate { get; set; }

        public ModificationRequestStatus Status { get; set; } = ModificationRequestStatus.Pending;

        public ModificationField Field { get; set; }

        public string OldValue { get; set; } = "";
        public string NewValue { get; set; } = "";
    }


}

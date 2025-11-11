using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Enums.Specifics;
using Entity.Models.Base;

namespace Entity.Models.Operational
{
    /// <summary>
    /// Solicitud de modificación de datos.
    /// </summary>
    public class ModificationRequest : BaseModel
    {
        public int UserId { get; set; }
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Estado de la solicitud
        /// </summary>
        public ModificationRequestStatus Status { get; set; } = ModificationRequestStatus.Pending;

        /// <summary>
        /// Campo a modificar (Name, Email, etc.)
        /// </summary>
        public ModificationField Field { get; set; }
        public ModificationReason Reason { get; set; }       // Motivo de la solicitud

        public string OldValue { get; set; } = "";
        public string NewValue { get; set; } = "";
        public string? Message { get; set; }

        public int? UpdatedById { get; set; }                // FK lógica (Admin/User que actualiza)

        public User User { get; set; }

    }
}

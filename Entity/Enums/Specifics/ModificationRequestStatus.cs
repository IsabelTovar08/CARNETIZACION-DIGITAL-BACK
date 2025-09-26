using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Enums.Attributes;

namespace Entity.Enums.Specifics
{
    /// <summary>
    /// Estados posibles de una solicitud de modificación.
    /// </summary>
    public enum ModificationRequestStatus
    {
        [EnumDisplayEx("Pendiente")]
        Pending = 0,

        [EnumDisplayEx("Aprobado")]
        Approved = 1,

        [EnumDisplayEx("Rechazado")]
        Rejected = 2
    }

}

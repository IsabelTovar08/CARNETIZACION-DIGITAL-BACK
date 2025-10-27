using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Enums.Attributes;

namespace Utilities.Enums.Specifics
{
    /// <summary>
    /// Estados posibles de una notificación recibida por un usuario.
    /// </summary>
    public enum NotificationStatus
    {
        [EnumDisplayEx("Enviado")]
        Sent = 1,

        [EnumDisplayEx("Leído")]
        Read = 2
    }
}

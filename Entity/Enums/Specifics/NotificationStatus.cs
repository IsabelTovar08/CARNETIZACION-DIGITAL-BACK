using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Enums.Attributes;

namespace Utilities.Enums.Specifics
{
    public enum NotificationStatus
    {
        [EnumDisplayEx("Pendiente")]
        Pending = 1,

        [EnumDisplayEx("Enviado")]
        Sent = 2,

        [EnumDisplayEx("Leído")]
        Read = 3,

        [EnumDisplayEx("Caducado")]
        Expired = 4
    }
}

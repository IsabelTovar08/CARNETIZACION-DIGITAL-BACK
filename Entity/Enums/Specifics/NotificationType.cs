using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Enums.Attributes;

namespace Utilities.Enums.Specifics
{
    public enum NotificationType
    {
        [EnumDisplayEx("Sistema")]
        System = 1,

        [EnumDisplayEx("Recordatorio")]
        Reminder = 2,

        [EnumDisplayEx("Advertencia")]
        Warning = 3,

        [EnumDisplayEx("Información")]
        Info = 4
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums.Attributes;

namespace Utilities.Enums.Specifics
{
    /// <summary>
    /// Tipos de punto de acceso. 
    /// </summary>
    public enum AccessPointType
    {
        [EnumDisplayEx("Entrada")]
        Entry = 1,

        [EnumDisplayEx("Salida")]
        Exit = 2,

        [EnumDisplayEx("Entrada y salida")]
        Bidirectional = 3
    }
}

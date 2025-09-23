using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Enums.Attributes;

namespace Utilities.Enums.Specifics
{
    /// <summary>
    /// Tipos de documento. 
    /// </summary>
    public enum DocumentType
    {
        [EnumDisplayEx("Cédula de ciudadanía", "CC")]
        CC = 1,

        [EnumDisplayEx("Cédula de extranjería", "CE")]
        CE = 2,

        [EnumDisplayEx("Tarjeta de identidad", "TI")]
        TI = 3,

        [EnumDisplayEx("Pasaporte", "PA")]
        PA = 4,

        [EnumDisplayEx("Número de Identificación Tributaria", "NIT")]
        NIT = 5
    }
}

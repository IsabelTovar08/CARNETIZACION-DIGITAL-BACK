using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Enums.Attributes;

namespace Entity.Enums.Specifics
{
    public enum ModificationField
    {
        [EnumDisplayEx("Nombre")]
        Name = 0,

        [EnumDisplayEx("Correo electrónico")]
        Email = 1,

        [EnumDisplayEx("Teléfono")]
        PhoneNumber = 2,

        [EnumDisplayEx("Dirección")]
        Address = 3,

        [EnumDisplayEx("Foto")]
        Photo = 4
    }

}

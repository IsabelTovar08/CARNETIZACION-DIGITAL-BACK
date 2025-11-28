using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Enums.Attributes;

namespace Entity.Enums.Specifics
{
    /// <summary>
    /// Motivos válidos y estandarizados para una solicitud de modificación.
    /// </summary>
    public enum ModificationReason
    {
        [EnumDisplayEx("Actualización de datos personales")]
        PersonalDataUpdate = 0,

        [EnumDisplayEx("Corrección de información incorrecta")]
        DataCorrection = 1,

        [EnumDisplayEx("Cambio oficial de documento o nombre")]
        OfficialChange = 2,

        [EnumDisplayEx("Actualización de contacto institucional")]
        ContactUpdate = 3
    }

}

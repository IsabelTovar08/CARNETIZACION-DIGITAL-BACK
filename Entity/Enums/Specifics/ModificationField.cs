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
        [EnumDisplayEx("Primer nombre")]
        FirstName = 0,

        [EnumDisplayEx("Segundo nombre")]
        MiddleName = 1,

        [EnumDisplayEx("Primer apellido")]
        LastName = 2,

        [EnumDisplayEx("Segundo apellido")]
        SecondLastName = 3,

        [EnumDisplayEx("Número de documento")]
        DocumentNumber = 4,

        [EnumDisplayEx("Correo electrónico")]
        Email = 5,

        [EnumDisplayEx("Teléfono")]
        Phone = 6,

        [EnumDisplayEx("Dirección")]
        Address = 7,

        [EnumDisplayEx("Tipo de documento")]
        DocumentTypeId = 8,

        [EnumDisplayEx("Tipo de sangre")]
        BloodTypeId = 9,

        [EnumDisplayEx("Foto de perfil (URL)")]
        PhotoUrl = 10,

        [EnumDisplayEx("Foto (ruta local)")]
        PhotoPath = 11,

        [EnumDisplayEx("Ciudad")]
        CityId = 12
    }

}

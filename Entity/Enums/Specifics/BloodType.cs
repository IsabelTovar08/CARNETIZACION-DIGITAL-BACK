using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Enums.Attributes;

namespace Utilities.Enums.Specifics
{
    /// <summary>
    /// Tipos de sangre. 
    /// </summary>
    public enum BloodType
    {
        [EnumDisplayEx("O+")] O_Positive = 1,
        [EnumDisplayEx("O-")] O_Negative = 2,
        [EnumDisplayEx("A+")] A_Positive = 3,
        [EnumDisplayEx("A-")] A_Negative = 4,
        [EnumDisplayEx("B+")] B_Positive = 5,
        [EnumDisplayEx("B-")] B_Negative = 6,
        [EnumDisplayEx("AB+")] AB_Positive = 7,
        [EnumDisplayEx("AB-")] AB_Negative = 8
    }
}

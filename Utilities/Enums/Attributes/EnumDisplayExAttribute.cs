using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Enums.Attributes
{
    /// <summary>
    /// Atributo para enriquecer enums con Nombre y, opcionalmente, Sigla (acrónimo).
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumDisplayExAttribute : Attribute
    {
        public string Name { get; }
        public string? Acronym { get; }

        public EnumDisplayExAttribute(string name, string? acronym = null)
        {
            Name = name;
            Acronym = acronym;
        }
    }
}

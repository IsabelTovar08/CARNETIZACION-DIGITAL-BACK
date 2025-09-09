using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums.Attributes;

namespace Utilities.Enums.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Obtiene el atributo EnumDisplayExAttribute de un valor enum.
        /// </summary>
        public static TAttribute? GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            return memberInfo?.GetCustomAttribute<TAttribute>();
        }

        /// <summary>
        /// Obtiene el nombre legible (Name).
        /// </summary>
        public static string GetDisplayName(this Enum value)
            => value.GetAttribute<EnumDisplayExAttribute>()?.Name ?? value.ToString();

        /// <summary>
        /// Obtiene el acrónimo (sigla), si existe.
        /// </summary>
        public static string? GetAcronym(this Enum value)
            => value.GetAttribute<EnumDisplayExAttribute>()?.Acronym;

        /// <summary>
        /// Convierte todos los valores de un enum a una lista con Id, Name y Acronym.
        /// </summary>
        public static IEnumerable<(int Id, string Name, string? Acronym)> ToItems<TEnum>() where TEnum : Enum
        {
            foreach (var v in Enum.GetValues(typeof(TEnum)).Cast<Enum>())
            {
                yield return (
                    Convert.ToInt32(v),
                    v.GetDisplayName(),
                    v.GetAcronym()
                );
            }
        }
    }
}

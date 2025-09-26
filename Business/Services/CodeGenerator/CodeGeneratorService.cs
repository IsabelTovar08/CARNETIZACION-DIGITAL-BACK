using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Entity.DTOs.Base;
using Entity.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.CodeGenerator
{
    public class CodeGeneratorService<T> : ICodeGeneratorService<T> where T : BaseModel
    {
        // Palabras a ignorar (es/en)
        private static readonly HashSet<string> Stop = new(StringComparer.OrdinalIgnoreCase)
        {
            "de","del","la","las","los","el","y","o","u","a",
            "the","of","for","and","to","in","on","at"
        };


        public async Task<string> EnsureCodeAsync(T entity, Func<string, int, Task<bool>> codeExists)
        {
            // Si ya tiene Code, no generamos
            if (!string.IsNullOrWhiteSpace(entity.Code))
                return entity.Code!;

            // Intentamos leer propiedad "Name" por reflexión (las entidades no lo traen en el BaseModel)
            var nameProp = entity.GetType().GetProperty("Name");
            var name = nameProp?.GetValue(entity) as string;

            if (string.IsNullOrWhiteSpace(name))
                return entity.Code ?? string.Empty; // no hay Name → no generamos

            var baseCode = BuildAcronym(name.Trim());
            if (string.IsNullOrWhiteSpace(baseCode)) baseCode = "ITEM";

            var finalCode = await MakeUniqueAsync(baseCode, entity.Id, codeExists);
            entity.Code = finalCode; // asignación directa (sí está en BaseModel)
            return finalCode;
        }

        // ✅ Acrónimo con más letras para disminuir colisiones:
        // - 1 token: primeros 6 chars (ej: "Universidad" -> "UNIVER")
        // - 2 tokens: 3+3 (ej: "Universidad Nacional" -> "UNINAC")
        // - 3 tokens: 2+2+2 (ej: "Uni Norma Norte" -> "UNNORN")
        // - 4+ tokens: 2 letras de los 4 primeros (total 8)
        // Siempre en MAYÚSCULAS, sin acentos, solo A-Z0-9, máx 12.
        public static string BuildAcronym(string name, int maxLen = 12)
        {
            string normalized = RemoveDiacritics(name).Trim();
            var tokens = Regex.Split(normalized, @"[^A-Za-z0-9]+")
                              .Where(t => !string.IsNullOrWhiteSpace(t))
                              .Where(t => !Stop.Contains(t))
                              .ToList();

            if (tokens.Count == 0)
            {
                // Si todo eran stopwords: usa el primer “trozo” crudo
                var raw = Regex.Split(RemoveDiacritics(name), @"[^A-Za-z0-9]+")
                               .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t)) ?? "ITEM";
                return Take(raw.ToUpperInvariant(), 6);
            }

            var parts = new List<string>();

            if (tokens.Count == 1)
            {
                parts.Add(Take(tokens[0].ToUpperInvariant(), 6));
            }
            else if (tokens.Count == 2)
            {
                parts.Add(Take(tokens[0].ToUpperInvariant(), 3));
                parts.Add(Take(tokens[1].ToUpperInvariant(), 3));
            }
            else if (tokens.Count == 3)
            {
                parts.Add(Take(tokens[0].ToUpperInvariant(), 2));
                parts.Add(Take(tokens[1].ToUpperInvariant(), 2));
                parts.Add(Take(tokens[2].ToUpperInvariant(), 2));
            }
            else
            {
                // 4 o más → 2 letras de los 4 primeros (8 en total)
                foreach (var t in tokens.Take(4))
                    parts.Add(Take(t.ToUpperInvariant(), 2));
            }

            var acronym = string.Concat(parts);
            acronym = Regex.Replace(acronym, @"[^A-Z0-9]", ""); // limpia
            if (acronym.Length > maxLen) acronym = acronym[..maxLen];
            return acronym;
        }

        private static async Task<string> MakeUniqueAsync(
            string baseCode, int currentId, Func<string, int, Task<bool>> codeExists)
        {
            string candidate = baseCode;
            int seq = 1;

            while (await codeExists(candidate, currentId))
            {
                candidate = $"{baseCode}-{seq.ToString("D3")}";
                seq++;
            }
            return candidate;
        }

        // Utilidades
        private static string Take(string s, int len) => s.Length <= len ? s : s[..len];

        private static string RemoveDiacritics(string text)
        {
            var formD = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(formD.Length);
            foreach (var ch in formD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark) sb.Append(ch);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}

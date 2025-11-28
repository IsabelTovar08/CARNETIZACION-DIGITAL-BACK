using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Helpers
{
    public static class GeneratePassword
    {
        /// <summary>
        /// Genera una contraseña temporal aleatoria.
        /// </summary>
        public static string GenerateTempPassword(int length = 10)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789@$!#%*?";
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                sb.Append(chars[bytes[i] % chars.Length]);
            return sb.ToString();
        }
    }
}

using System;

namespace Utilities.Helper
{
    public static class EncryptedPassword
    {
        public static string EncryptPassword(string password)
        {
            // Si ya está hasheada, no volver a hashearla
            if (IsBcryptHash(password))
            {
                return password;
            }

            return BCrypt.Net.BCrypt.HashPassword(password,workFactor: 12);
        }

        public static bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            if (IsBcryptHash(hashedPassword))
            {
                // Si el hash es válido (es BCrypt), usar la verificación segura
                return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
            }

            // Si no es un hash, se asume texto plano → comparar directamente
            return plainPassword == hashedPassword;
        }

        private static bool IsBcryptHash(string hash)
        {
            return !string.IsNullOrWhiteSpace(hash)
                && (hash.StartsWith("$2a$") || hash.StartsWith("$2b$") || hash.StartsWith("$2y$"))
                && hash.Length == 60;
        }
    }
}

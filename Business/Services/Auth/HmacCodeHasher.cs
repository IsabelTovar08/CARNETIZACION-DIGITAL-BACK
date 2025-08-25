using Business.Interfaces.Auth;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Auth

// Este servicio utiliza HMAC con SHA-256
// para hashear y verificar códigos de verificación.
{
    public class HmacCodeHasher : ICodeHasher
    {
        private readonly byte[] _key;
        public HmacCodeHasher(IConfiguration cfg)
        {
            var key = cfg["Codes:HmacKey"];
            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException
                    ("Falta Codes:HmacKey en la configuración. Agrega la sección Codes al appsettings.");
            _key = Encoding.UTF8.GetBytes(key);
        }
        public string Hash(string code)
        {
            using var h = new System.Security.Cryptography.HMACSHA256(_key);
            return Convert.ToBase64String(h.ComputeHash(Encoding.UTF8.GetBytes(code)));
        }

        public bool Verify(string code, string hash)
        {
            var computed = Hash(code);
            return System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(computed),
                Convert.FromBase64String(hash)
            );
        }

    }
}

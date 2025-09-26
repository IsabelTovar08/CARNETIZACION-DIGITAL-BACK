using Business.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Este servicio lo que hace es generar un código de 5 dígitos
// aleatorios para la verificación del usuario.

namespace Business.Services.Auth
{
    public class FiveDigitCodeGenerator : ICodeGenerator
    {
        private static readonly System.Security.Cryptography.RandomNumberGenerator Rng =
            System.Security.Cryptography.RandomNumberGenerator.Create();
        public string Generate(int digits = 5)
        {
            var b = new byte[4]; Rng.GetBytes(b);
            var v = BitConverter.ToUInt32(b, 0) % (uint)Math.Pow(10, digits);
            return v.ToString(new string('0', digits));
        }
    }
}

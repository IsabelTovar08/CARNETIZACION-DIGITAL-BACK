using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Helpers.Images
{
    /// <summary>
    /// Valida y normaliza el formato de imagen por "magic numbers" y/o extensión.
    ///     - Soporta PNG, JPG/JPEG, GIF, BMP.
    ///     - Lanza InvalidOperationException con mensaje en español si no es válido.
    /// </summary>
    public static class ImageFormatValidator
    {
        // Firmas (magic numbers)
        private static readonly byte[] PngSig = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        private static readonly byte[] JpegSig = new byte[] { 0xFF, 0xD8 };               // SOI
        private static readonly byte[] Gif87a = System.Text.Encoding.ASCII.GetBytes("GIF87a");
        private static readonly byte[] Gif89a = System.Text.Encoding.ASCII.GetBytes("GIF89a");
        private static readonly byte[] BmpSig = new byte[] { 0x42, 0x4D };               // "BM"

        /// <summary>
        /// Retorna extensión y content-type normalizados si el formato es soportado.
        /// </summary>
        public static (string normalizedExt, string contentType) EnsureSupported(byte[] bytes, string? extHint)
        {
            if (bytes == null || bytes.Length < 4)
                throw new InvalidOperationException("La imagen está vacía o es inválida.");

            // Preferimos detección por bytes
            if (StartsWith(bytes, PngSig)) return (".png", "image/png");
            if (StartsWith(bytes, JpegSig)) return (".jpg", "image/jpeg");
            if (StartsWith(bytes, Gif87a) || StartsWith(bytes, Gif89a)) return (".gif", "image/gif");
            if (StartsWith(bytes, BmpSig)) return (".bmp", "image/bmp");

            // Fallback por extensión de pista
            var ext = (extHint ?? string.Empty).Trim().ToLowerInvariant();
            return ext switch
            {
                ".png" => (".png", "image/png"),
                ".jpg" or ".jpeg" => (".jpg", "image/jpeg"),
                ".gif" => (".gif", "image/gif"),
                ".bmp" => (".bmp", "image/bmp"),
                _ => throw new InvalidOperationException("Formato de imagen no soportado. Usa PNG, JPG, GIF o BMP.")
            };
        }

        /// <summary>
        /// Versión Try: no lanza excepción; devuelve false si no es soportado.
        /// </summary>
        public static bool TryEnsureSupported(byte[] bytes, string? extHint, out string normalizedExt, out string contentType)
        {
            normalizedExt = ".jpg";
            contentType = "image/jpeg";

            if (bytes == null || bytes.Length < 4) return false;

            if (StartsWith(bytes, PngSig)) { normalizedExt = ".png"; contentType = "image/png"; return true; }
            if (StartsWith(bytes, JpegSig)) { normalizedExt = ".jpg"; contentType = "image/jpeg"; return true; }
            if (StartsWith(bytes, Gif87a) || StartsWith(bytes, Gif89a))
            { normalizedExt = ".gif"; contentType = "image/gif"; return true; }
            if (StartsWith(bytes, BmpSig)) { normalizedExt = ".bmp"; contentType = "image/bmp"; return true; }

            var ext = (extHint ?? string.Empty).Trim().ToLowerInvariant();
            switch (ext)
            {
                case ".png": normalizedExt = ".png"; contentType = "image/png"; return true;
                case ".jpg":
                case ".jpeg": normalizedExt = ".jpg"; contentType = "image/jpeg"; return true;
                case ".gif": normalizedExt = ".gif"; contentType = "image/gif"; return true;
                case ".bmp": normalizedExt = ".bmp"; contentType = "image/bmp"; return true;
                default: return false;
            }
        }

        // ---------- Privado ----------
        private static bool StartsWith(byte[] data, byte[] sig)
        {
            if (data.Length < sig.Length) return false;
            for (int i = 0; i < sig.Length; i++)
                if (data[i] != sig[i]) return false;
            return true;
        }
    }
}

using System;
using System.IO;
using QRCoder;

namespace Utilities.Helpers
{
    /// <summary>
    /// Generador genérico de QR: recibe cualquier string y lo convierte a PNG (bytes/base64) o SVG.
    /// Sin dependencias de capa negocio; utilitario puro.
    /// </summary>
    public static class QrCodeHelper
    {
        /// <summary>Devuelve PNG (bytes) del contenido.</summary>
        public static byte[] ToPngBytes(string content, int pixelsPerModule = 10, bool drawQuietZones = true)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("El contenido del QR no puede ser vacío.", nameof(content));

            using var gen = new QRCodeGenerator();
            using var data = gen.CreateQrCode(content, QRCodeGenerator.ECCLevel.M);
            using var png = new PngByteQRCode(data);
            return png.GetGraphic(pixelsPerModule, System.Drawing.Color.Black, System.Drawing.Color.White, drawQuietZones);
        }

        /// <summary>Devuelve PNG en Base64 del contenido.</summary>
        public static string ToPngBase64(string content, int pixelsPerModule = 10, bool drawQuietZones = true)
        {
            var bytes = ToPngBytes(content, pixelsPerModule, drawQuietZones);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>Devuelve Data URL (image/png;base64,...)</summary>
        public static string ToDataUrl(string content, int pixelsPerModule = 10, bool drawQuietZones = true)
        {
            var b64 = ToPngBase64(content, pixelsPerModule, drawQuietZones);
            return $"data:image/png;base64,{b64}";
        }

        /// <summary>Devuelve SVG como string.</summary>
        public static string ToSvg(string content, int pixelsPerModule = 10, bool drawQuietZones = true)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("El contenido del QR no puede ser vacío.", nameof(content));

            using var gen = new QRCodeGenerator();
            using var data = gen.CreateQrCode(content, QRCodeGenerator.ECCLevel.M);
            var svg = new SvgQRCode(data);
            return svg.GetGraphic(pixelsPerModule, System.Drawing.Color.Black, System.Drawing.Color.White, drawQuietZones);
        }

        /// <summary>Guarda un PNG en disco.</summary>
        public static void SavePng(string content, string filePath, int pixelsPerModule = 10, bool drawQuietZones = true)
        {
            var bytes = ToPngBytes(content, pixelsPerModule, drawQuietZones);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            File.WriteAllBytes(filePath, bytes);
        }
    }
}

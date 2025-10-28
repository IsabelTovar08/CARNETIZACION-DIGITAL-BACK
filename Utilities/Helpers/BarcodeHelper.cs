using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace Utilities.Helpers
{
    public static class BarcodeHelper
    {
        /// <summary>
        /// Genera un código QR como imagen SKImage.
        /// </summary>
        public static SKImage? GenerateQr(string content, int size = 200)
        {
            if (string.IsNullOrWhiteSpace(content))
                return null;

            try
            {
                var writer = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        Width = size,
                        Height = size,
                        Margin = 1
                    }
                };

                var pixelData = writer.Write(content);

                // Crear SKImage seguro (sin unsafe)
                using var bitmap = new SKBitmap(pixelData.Width, pixelData.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
                var span = bitmap.GetPixelSpan();
                pixelData.Pixels.CopyTo(span); // copia segura sin punteros

                return SKImage.FromBitmap(bitmap);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Genera un QR como arreglo de bytes PNG.
        /// </summary>
        public static byte[]? GenerateQrBytes(string content, int size = 200)
        {
            using var image = GenerateQr(content, size);
            if (image == null) return null;

            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }

        /// <summary>
        /// Genera un QR como cadena Base64 (PNG).
        /// </summary>
        public static string? GenerateQrBase64(string content, int size = 200)
        {
            var bytes = GenerateQrBytes(content, size);
            return bytes != null ? Convert.ToBase64String(bytes) : null;
        }

        // ===========================================================
        // 🔹 BARCODE (CODE_128)
        // ===========================================================

        /// <summary>
        /// Genera un código de barras CODE_128 como imagen SKImage.
        /// </summary>
        public static SKImage? GenerateBarcode(string content, int width = 220, int height = 60)
        {
            if (string.IsNullOrWhiteSpace(content))
                return null;

            try
            {
                var writer = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new EncodingOptions
                    {
                        Width = width,
                        Height = height,
                        Margin = 1,
                        PureBarcode = true
                    }
                };

                var pixelData = writer.Write(content);

                // Crear SKImage seguro (sin unsafe)
                using var bitmap = new SKBitmap(pixelData.Width, pixelData.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
                var span = bitmap.GetPixelSpan();
                pixelData.Pixels.CopyTo(span);

                return SKImage.FromBitmap(bitmap);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Genera un código de barras CODE_128 como arreglo de bytes (PNG).
        /// </summary>
        public static byte[]? GenerateBarcodeBytes(string content, int width = 220, int height = 60)
        {
            using var image = GenerateBarcode(content, width, height);
            if (image == null) return null;

            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }

        /// <summary>
        /// Genera un código de barras CODE_128 como cadena Base64 (PNG).
        /// </summary>
        public static string? GenerateBarcodeBase64(string content, int width = 220, int height = 60)
        {
            var bytes = GenerateBarcodeBytes(content, width, height);
            return bytes != null ? Convert.ToBase64String(bytes) : null;
        }
    }
}

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

                // Copia los píxeles de forma segura sin punteros (compatible con SkiaSharp 2.88.8)
                var handle = bitmap.GetPixels();
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, handle, pixelData.Width * pixelData.Height * 4);

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

                // Copia los píxeles de forma segura sin punteros (compatible con SkiaSharp 2.88.8)
                var handle = bitmap.GetPixels();
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, handle, pixelData.Width * pixelData.Height * 4);

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



        /// <summary>
        /// Limpia un string Base64 eliminando prefijos como data:image/png;base64.
        /// </summary>
        public static string CleanBase64Logo(string? rawBase64)
        {
            if (string.IsNullOrWhiteSpace(rawBase64))
                return string.Empty;

            // Si viene con metadata tipo "data:image/png;base64,xxxx"
            int commaIndex = rawBase64.IndexOf(',');

            return commaIndex >= 0
                ? rawBase64[(commaIndex + 1)..].Trim()
                : rawBase64.Trim();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Specifics.Cards;
using Entity.Models.Operational;
using QRCoder;
using SkiaSharp;
using Svg.Skia;
using Utilities.Helpers;

namespace Business.Services.Cards
{
    /// <summary>
    /// Servicio que genera el PDF de carnets (frontal y posterior)
    /// usando SkiaSharp y Svg.Skia.
    /// Ajustado a tamaño real (8.6 x 5.4 cm) con mejor calidad de imagen.
    /// </summary>
    public class CardPdfService : ICardPdfService
    {
        private readonly HttpClient _http;

        public CardPdfService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        /// <summary>
        /// Genera el carnet en formato PDF con frente y reverso.
        /// </summary>
        public async Task GenerateCardAsync(CardTemplateResponse template, CardUserData userData, Stream output)
        {
            // === 1️⃣ Cargar fondos SVG ===
            var frontSvgBytes = await _http.GetByteArrayAsync(template.FrontBackgroundUrl);
            var backSvgBytes = await _http.GetByteArrayAsync(template.BackBackgroundUrl);

            using var frontSvg = new SKSvg();
            using var backSvg = new SKSvg();
            using (var fs = new MemoryStream(frontSvgBytes)) frontSvg.Load(fs);
            using (var bs = new MemoryStream(backSvgBytes)) backSvg.Load(bs);

            var frontPos = JsonSerializer.Deserialize<Dictionary<string, Position>>(template.FrontElementsJson)
                           ?? throw new InvalidOperationException("FrontElementsJson inválido.");

            var backPos = JsonSerializer.Deserialize<Dictionary<string, Position>>(template.BackElementsJson)
                          ?? throw new InvalidOperationException("BackElementsJson inválido.");

            // === 2️⃣ Cargar imágenes ===
            using var userPhoto = await LoadImageAsync(userData.UserPhotoUrl);
            using var logoImg = await LoadImageAsync(userData.LogoUrl);
            using var qrImg = BarcodeHelper.GenerateQr(userData.UniqueId.ToString(), 160); // QR dinámico
            using var barcodeImg = BarcodeHelper.GenerateBarcode(userData.UniqueId.ToString(), 160, 40); // Código de barras por UUID

            // === 3️⃣ Tamaño real del carnet ===
            const float CARD_WIDTH_CM = 8.6f;
            const float CARD_HEIGHT_CM = 5.4f;
            const float widthPoints = CARD_WIDTH_CM / 2.54f * 72f;   // ≈ 244.8 pt
            const float heightPoints = CARD_HEIGHT_CM / 2.54f * 72f; // ≈ 153.6 pt

            using var pdf = SKDocument.CreatePdf(output);

            // === 4️⃣ Página frontal ===
            using (var canvas = pdf.BeginPage(widthPoints, heightPoints))
            {
                DrawSvg(canvas, frontSvg, widthPoints, heightPoints);

                // Fondo base
                canvas.Clear(SKColors.White);
                canvas.Flush();

                // Imágenes principales
                DrawImage(canvas, logoImg, frontPos, "logo");
                DrawImage(canvas, userPhoto, frontPos, "userPhoto");
                DrawImage(canvas, qrImg, frontPos, "qr");

                // Textos principales
                DrawText(canvas, userData.CompanyName?.ToUpper(), frontPos, "companyName", 18, bold: true, color: SKColors.White);
                DrawText(canvas, userData.Profile?.ToUpper(), frontPos, "profile", 16, bold: true, color: SKColors.Black);

                DrawText(canvas, userData.Name, frontPos, "name", 20, bold: true);
                DrawText(canvas, userData.CategoryArea, frontPos, "categoryArea", 14, color: new SKColor(70, 70, 70));
                DrawText(canvas, userData.PhoneNumber, frontPos, "phoneNumber", 12);
                DrawText(canvas, userData.Email, frontPos, "email", 12);

                // ID destacado
                if (frontPos.TryGetValue("cardId", out var idPos))
                {
                    DrawRoundedBox(canvas, idPos.x - 10, idPos.y - 20, 130, 30, SKColors.LightBlue);
                    DrawText(canvas, $"#ID: {userData.CardId}", frontPos, "cardId", 16, bold: true, color: SKColors.DarkBlue);
                }

                pdf.EndPage();
            }

            // === 5️⃣ Página reversa ===
            using (var canvas = pdf.BeginPage(widthPoints, heightPoints))
            {
                DrawSvg(canvas, backSvg, widthPoints, heightPoints);

                // Términos básicos
                DrawText(canvas, "Términos y Condiciones", backPos, "title", 14, bold: true);
                DrawText(canvas, "Este carnet debe presentarse al ingresar o solicitar servicios internos.", backPos, "info1", 10);
                DrawText(canvas, "Fraude o alteración puede conllevar sanciones.", backPos, "info2", 10);
                DrawText(canvas, $"Contacto: {userData.Email}", backPos, "email", 10);
                DrawText(canvas, DateTime.Now.ToString("dd/MM/yyyy"), backPos, "date", 9, color: SKColors.Gray);

                // Código de barras centrado
                if (barcodeImg != null)
                {
                    float x = (widthPoints - 160) / 2f;
                    float y = heightPoints - 50;
                    canvas.DrawImage(barcodeImg, new SKRect(x, y, x + 160, y + 40));
                }

                pdf.EndPage();
            }

            pdf.Close();
        }

        // ===========================================================
        // 🔹 Métodos auxiliares
        // ===========================================================

        private sealed class Position
        {
            public float x { get; set; }
            public float y { get; set; }
            public float? width { get; set; }
            public float? height { get; set; }
        }

        /// <summary>
        /// Carga una imagen remota y la decodifica en alta calidad.
        /// </summary>
        private async Task<SKImage?> LoadImageAsync(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;
            try
            {
                var bytes = await _http.GetByteArrayAsync(url);
                using var ms = new MemoryStream(bytes);
                using var bitmap = await Task.Run(() => SKBitmap.Decode(ms));
                return SKImage.FromBitmap(bitmap);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Dibuja una imagen en la posición indicada.
        /// </summary>
        private void DrawImage(SKCanvas canvas, SKImage? img, IDictionary<string, Position> posMap, string key)
        {
            if (img == null || !posMap.TryGetValue(key, out var pos)) return;

            using var paint = new SKPaint
            {
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High
            };

            var rect = new SKRect(pos.x, pos.y, pos.x + (pos.width ?? img.Width), pos.y + (pos.height ?? img.Height));
            canvas.DrawImage(img, rect, paint);
        }

        /// <summary>
        /// Dibuja texto con color y estilo opcional.
        /// </summary>
        private void DrawText(SKCanvas canvas, string? text, IDictionary<string, Position> posMap, string key, float fontSize, bool bold = false, SKColor? color = null)
        {
            if (string.IsNullOrEmpty(text) || !posMap.TryGetValue(key, out var pos)) return;

            using var paint = new SKPaint
            {
                Color = color ?? SKColors.Black,
                TextSize = fontSize,
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High,
                Typeface = SKFontManager.Default.MatchFamily("Arial", bold ? SKFontStyle.Bold : SKFontStyle.Normal)
            };

            canvas.DrawText(text, pos.x, pos.y, paint);
        }

        /// <summary>
        /// Dibuja un rectángulo redondeado (para destacar el ID).
        /// </summary>
        private void DrawRoundedBox(SKCanvas canvas, float x, float y, float width, float height, SKColor color)
        {
            using var paint = new SKPaint { Color = color, IsAntialias = true };
            var rect = new SKRoundRect(new SKRect(x, y, x + width, y + height), 8, 8);
            canvas.DrawRoundRect(rect, paint);
        }

        /// <summary>
        /// Dibuja el SVG escalado proporcionalmente al lienzo.
        /// </summary>
        private void DrawSvg(SKCanvas canvas, SKSvg svg, float width, float height)
        {
            var picture = svg.Picture;
            if (picture == null) return;

            var src = picture.CullRect;
            var scale = Math.Min(width / src.Width, height / src.Height);
            var tx = (width - src.Width * scale) / 2f;
            var ty = (height - src.Height * scale) / 2f;

            canvas.Save();
            canvas.Translate(tx, ty);
            canvas.Scale(scale);
            canvas.DrawPicture(picture);
            canvas.Restore();
        }
    }
}

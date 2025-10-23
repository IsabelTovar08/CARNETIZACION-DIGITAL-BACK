using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Specifics.Cards;
using Entity.Models.Operational;
using SkiaSharp;
using Svg.Skia;

namespace Business.Services.Cards
{
    /// <summary>
    /// Servicio que genera el PDF de carnets (frontal y posterior)
    /// usando exclusivamente SkiaSharp y Svg.Skia.
    /// </summary>
    public class CardPdfService : ICardPdfService
    {
        private readonly HttpClient _http;

        public CardPdfService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        /// <summary>
        /// Genera un carnet PDF más estilizado (basado en tu plantilla SENA).
        /// </summary>
        public async Task GenerateCardAsync(CardTemplateResponse template, CardUserData userData, Stream output)
        {
            // === 1️⃣ Cargar fondos ===
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

            using var userPhoto = await LoadImageAsync(userData.UserPhotoUrl);
            using var logoImg = await LoadImageAsync(userData.LogoUrl);
            using var qrImg = await LoadImageAsync(userData.QrUrl);

            var (pageW, pageH) = GetPageSizePoints(frontSvg);

            using var pdf = SKDocument.CreatePdf(output);
            using (var canvas = pdf.BeginPage(pageW, pageH))
            {
                DrawSvg(canvas, frontSvg, pageW, pageH);

                // === Fondo y estructura ===
                DrawImage(canvas, logoImg, frontPos, "logo");
                DrawImage(canvas, userPhoto, frontPos, "userPhoto");
                DrawImage(canvas, qrImg, frontPos, "qr");

                // === Encabezado ===
                DrawText(canvas, userData.CompanyName.ToUpper(), frontPos, "companyName", 18, bold: true, color: SKColors.White);
                DrawText(canvas, userData.Profile.ToUpper(), frontPos, "profile", 16, bold: true, color: SKColors.Black);

                // === Datos personales ===
                DrawText(canvas, userData.Name, frontPos, "name", 20, bold: true);
                DrawText(canvas, userData.CategoryArea, frontPos, "categoryArea", 14, color: new SKColor(70, 70, 70));
                DrawText(canvas, userData.PhoneNumber, frontPos, "phoneNumber", 12, color: SKColors.Black);
                DrawText(canvas, userData.Email, frontPos, "email", 12, color: SKColors.Black);

                // === ID destacado ===
                DrawRoundedBox(canvas, frontPos["cardId"].x - 10, frontPos["cardId"].y - 20, 130, 30, SKColors.LightBlue);
                DrawText(canvas, $"#ID: {userData.CardId}", frontPos, "cardId", 16, bold: true, color: SKColors.DarkBlue);

                pdf.EndPage();
            }

            // === Reverso (simple informativo) ===
            using (var canvas = pdf.BeginPage(pageW, pageH))
            {
                DrawSvg(canvas, backSvg, pageW, pageH);

                DrawText(canvas, "Información del titular", backPos, "title", 14, bold: true);
                DrawText(canvas, $"Tel: {userData.PhoneNumber}", backPos, "phoneNumber", 12);
                DrawText(canvas, $"Email: {userData.Email}", backPos, "email", 12);
                DrawText(canvas, DateTime.Now.ToString("dd/MM/yyyy"), backPos, "address", 10, color: SKColors.Gray);

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

        private async Task<SKImage?> LoadImageAsync(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;
            try
            {
                var bytes = await _http.GetByteArrayAsync(url);
                using var data = SKData.CreateCopy(bytes);
                return SKImage.FromEncodedData(data);
            }
            catch { return null; }
        }

        private void DrawImage(SKCanvas canvas, SKImage? img, IDictionary<string, Position> posMap, string key)
        {
            if (img == null || !posMap.TryGetValue(key, out var pos)) return;

            var rect = new SKRect(pos.x, pos.y, pos.x + (pos.width ?? img.Width), pos.y + (pos.height ?? img.Height));
            canvas.DrawImage(img, rect);
        }

        /// <summary>
        /// Dibuja un texto con soporte de color y negrita.
        /// </summary>
        private void DrawText(SKCanvas canvas, string? text, IDictionary<string, Position> posMap, string key, float fontSize, bool bold = false, SKColor? color = null)
        {
            if (string.IsNullOrEmpty(text) || !posMap.TryGetValue(key, out var pos)) return;

            using var paint = new SKPaint
            {
                Color = color ?? SKColors.Black,
                TextSize = fontSize,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial", bold ? SKFontStyle.Bold : SKFontStyle.Normal)
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

        private (float width, float height) GetPageSizePoints(SKSvg svg)
        {
            var pic = svg.Picture;
            if (pic != null && pic.CullRect.Width > 0 && pic.CullRect.Height > 0)
                return (pic.CullRect.Width, pic.CullRect.Height);

            // A7 portrait
            return (MmToPt(74), MmToPt(105));
        }

        private static float MmToPt(float mm) => mm * 72f / 25.4f;
    }
}

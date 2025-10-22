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
    /// usando exclusivamente SkiaSharp y Svg.Skia (versiones actuales).
    /// </summary>
    public class CardPdfService : ICardPdfService
    {
        private readonly HttpClient _http;

        public CardPdfService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        /// <summary>
        /// Genera un carnet en formato PDF dentro del Stream indicado.
        /// </summary>
        public async Task GenerateCardAsync(CardTemplateResponse template, CardUserData userData, Stream output)
        {
            // === 1️⃣ Descargar fondos SVG ===
            var frontSvgBytes = await _http.GetByteArrayAsync(template.FrontBackgroundUrl);
            var backSvgBytes = await _http.GetByteArrayAsync(template.BackBackgroundUrl);

            // === 2️⃣ Cargar SVGs ===
            using var frontSvg = new SKSvg();
            using var backSvg = new SKSvg();
            using (var fs = new MemoryStream(frontSvgBytes)) frontSvg.Load(fs);
            using (var bs = new MemoryStream(backSvgBytes)) backSvg.Load(bs);

            // === 3️⃣ Cargar posiciones desde JSON ===
            var frontPos = JsonSerializer.Deserialize<Dictionary<string, Position>>(template.FrontElementsJson)
                           ?? throw new InvalidOperationException("FrontElementsJson inválido.");
            var backPos = JsonSerializer.Deserialize<Dictionary<string, Position>>(template.BackElementsJson)
                          ?? throw new InvalidOperationException("BackElementsJson inválido.");

            // === 4️⃣ Cargar imágenes dinámicas (logo, foto, QR) ===
            using var userPhoto = await LoadImageAsync(userData.UserPhotoUrl);
            using var logoImg = await LoadImageAsync(userData.LogoUrl);
            using var qrImg = await LoadImageAsync(userData.QrUrl);

            // === 5️⃣ Crear documento PDF ===
            var (pageW, pageH) = GetPageSizePoints(frontSvg);

            using var pdf = SKDocument.CreatePdf(output);
            if (pdf == null)
                throw new InvalidOperationException("No se pudo crear el documento PDF.");

            // === Página frontal ===
            using (var canvas = pdf.BeginPage(pageW, pageH))
            {
                DrawSvg(canvas, frontSvg, pageW, pageH);

                // Imágenes
                DrawImage(canvas, logoImg, frontPos, "logo");
                DrawImage(canvas, userPhoto, frontPos, "userPhoto");
                DrawImage(canvas, qrImg, frontPos, "qr");

                // Textos
                DrawText(canvas, userData.CompanyName, frontPos, "companyName", 12);
                DrawText(canvas, userData.Name, frontPos, "name", 14, bold: true);
                DrawText(canvas, userData.Profile, frontPos, "profile", 12);
                DrawText(canvas, userData.CategoryArea, frontPos, "categoryArea", 12);
                DrawText(canvas, userData.PhoneNumber, frontPos, "phoneNumber", 10);
                DrawText(canvas, userData.Email, frontPos, "email", 10);
                DrawText(canvas, userData.CardId, frontPos, "cardId", 10);

                pdf.EndPage();
            }

            // === Página reverso ===
            using (var canvas = pdf.BeginPage(pageW, pageH))
            {
                DrawSvg(canvas, backSvg, pageW, pageH);

                DrawText(canvas, userData.Title, backPos, "title", 12, bold: true);
                DrawText(canvas, userData.Address, backPos, "address", 10);
                DrawText(canvas, userData.PhoneNumber, backPos, "phoneNumber", 10);
                DrawText(canvas, userData.Email, backPos, "email", 10);

                pdf.EndPage();
            }

            // === 6️⃣ Cerrar PDF ===
            pdf.Close();
        }

        // ===========================================================
        // 🔹 Métodos auxiliares
        // ===========================================================

        /// <summary>
        /// Clase auxiliar que representa coordenadas (coincide con tu JSON).
        /// </summary>
        private sealed class Position
        {
            public float x { get; set; }
            public float y { get; set; }
            public float? width { get; set; }
            public float? height { get; set; }
        }

        /// <summary>
        /// Descarga una imagen (JPG/PNG) o devuelve null si falla.
        /// </summary>
        private async Task<SKImage?> LoadImageAsync(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;
            try
            {
                var bytes = await _http.GetByteArrayAsync(url);
                using var data = SKData.CreateCopy(bytes);
                return SKImage.FromEncodedData(data);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Dibuja una imagen en el canvas según coordenadas definidas.
        /// </summary>
        private void DrawImage(SKCanvas canvas, SKImage? img, IDictionary<string, Position> posMap, string key)
        {
            if (img == null || !posMap.TryGetValue(key, out var pos)) return;

            if (pos.width.HasValue && pos.height.HasValue)
            {
                var rect = new SKRect(pos.x, pos.y, pos.x + pos.width.Value, pos.y + pos.height.Value);
                canvas.DrawImage(img, rect);
            }
            else
            {
                canvas.DrawImage(img, pos.x, pos.y);
            }
        }

        /// <summary>
        /// Dibuja un texto simple (Arial) en el canvas.
        /// </summary>
        private void DrawText(SKCanvas canvas, string? text, IDictionary<string, Position> posMap, string key, float fontSize, bool bold = false)
        {
            if (string.IsNullOrEmpty(text) || !posMap.TryGetValue(key, out var pos)) return;

            using var paint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = fontSize,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial", bold ? SKFontStyle.Bold : SKFontStyle.Normal)
            };

            canvas.DrawText(text, pos.x, pos.y, paint);
        }

        /// <summary>
        /// Dibuja el fondo SVG adaptado al tamaño de la página.
        /// </summary>
        private void DrawSvg(SKCanvas canvas, SKSvg svg, float width, float height)
        {
            var picture = svg.Picture;
            if (picture == null) return;

            var src = picture.CullRect;
            var scaleX = width / src.Width;
            var scaleY = height / src.Height;
            var scale = Math.Min(scaleX, scaleY);

            var tx = (width - src.Width * scale) / 2f;
            var ty = (height - src.Height * scale) / 2f;

            canvas.Save();
            canvas.Translate(tx, ty);
            canvas.Scale(scale);
            canvas.DrawPicture(picture);
            canvas.Restore();
        }

        /// <summary>
        /// Obtiene el tamaño de página desde el SVG o usa A7 horizontal por defecto.
        /// </summary>
        private (float width, float height) GetPageSizePoints(SKSvg svg)
        {
            var pic = svg.Picture;
            if (pic != null && pic.CullRect.Width > 0 && pic.CullRect.Height > 0)
                return (pic.CullRect.Width, pic.CullRect.Height);

            // A7 Landscape (105mm x 74mm)
            return (MmToPt(105), MmToPt(74));
        }

        /// <summary>
        /// Convierte milímetros a puntos (1 in = 25.4 mm, 1 in = 72 pt).
        /// </summary>
        private static float MmToPt(float mm) => mm * 72f / 25.4f;
    }
}

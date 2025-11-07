using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Specifics.Cards;
using SkiaSharp;
using Svg.Skia;
using Utilities.Helpers;

namespace Business.Services.Cards
{
    /// <summary>
    /// Genera carnet PDF (vertical 85.6x54 mm) con SVG al 100% del tamaño del PDF.
    /// Frente con mejoras visuales; reverso restaurado.
    /// </summary>
    public class CardPdfService : ICardPdfService
    {
        private readonly HttpClient _http;

        public CardPdfService(HttpClient httpClient) => _http = httpClient;

        public async Task GenerateCardAsync(CardTemplateResponse template, CardUserData userData, Stream output)
        {
            // Tamaño físico del carnet
            float pageW = MmToPt(54f);   // ~153 pt
            float pageH = MmToPt(85.6f); // ~243 pt

            // Base de diseño de posiciones
            float designWidth = 640f;
            float designHeight = 1010f;

            // Cargar fondos
            var frontSvgBytes = await _http.GetByteArrayAsync(template.FrontBackgroundUrl);
            var backSvgBytes = await _http.GetByteArrayAsync(template.BackBackgroundUrl);

            using var frontSvg = new SKSvg();
            using var backSvg = new SKSvg();
            using (var fs = new MemoryStream(frontSvgBytes)) frontSvg.Load(fs);
            using (var bs = new MemoryStream(backSvgBytes)) backSvg.Load(bs);

            // Imágenes
            using var logoImg = await LoadImageOrFallbackAsync(userData.LogoUrl, "Logo");
            using var userPhoto = await LoadImageOrFallbackAsync(userData.UserPhotoUrl, "Sin Foto");
            using var qrImg = DecodeBase64Image(userData.QrUrl);

            // Escala global de coordenadas → tamaño real
            float scaleX = pageW / designWidth;
            float scaleY = pageH / designHeight;
            float globalScale = Math.Min(scaleX, scaleY);

            using var pdf = SKDocument.CreatePdf(output);

            // ========================= FRONT =========================
            using (var canvas = pdf.BeginPage(pageW, pageH))
            {
                canvas.Clear(SKColors.White);

                // Fondo al 100% del PDF
                var pic = frontSvg.Picture;
                if (pic != null)
                {
                    var bounds = pic.CullRect;
                    canvas.Save();
                    canvas.Scale(pageW / bounds.Width, pageH / bounds.Height);
                    canvas.DrawPicture(pic);
                    canvas.Restore();
                }

                canvas.Scale(globalScale);
                var frontPos = GetFrontPositions();

                // Logo + company name (ajustado al chip verde)
                DrawImage(canvas, logoImg, frontPos["logo"]);
                DrawMultilineText(canvas, userData.CompanyName?.ToUpper(), frontPos["companyName"],
                    size: 26, bold: true, color: SKColors.White, maxWidth: 180);

                // Foto + QR
                DrawImage(canvas, userPhoto, frontPos["userPhoto"]);
                DrawImage(canvas, qrImg, frontPos["qr"]);

                // Nombre (nombres arriba, apellidos abajo) y más grande
                DrawMultilineText(canvas, userData.Name?.ToUpper(), frontPos["name"],
                    size: 38, bold: true, color: SKColors.Black, maxWidth: 260, lineSpacing: 40);

                // Identificación + División interna más grandes
                DrawText(canvas, $"CC: {userData.DocumentNumber}", frontPos["identification"], 30, true, new SKColor(40, 40, 40));
                DrawText(canvas, userData.InternalDivisionName, frontPos["internalDivision"], 30, false, SKColors.DarkSlateGray);

                // Unidad organizativa centrada (con sombra suave)
                DrawShadowedText(canvas, userData.CategoryArea, frontPos["categoryArea"], 26, false, new SKColor(50, 50, 50));

                // Perfil debajo del QR con sombra
                DrawShadowedText(canvas, userData.Profile?.ToUpper(), frontPos["profile"], 28, true, new SKColor(60, 60, 60));

                // Teléfono / Correo / RH
                var blue = new SKColor(0, 80, 160);
                DrawText(canvas, "Teléfono", frontPos["phoneLabel"], 26, true, blue);
                DrawText(canvas, userData.PhoneNumber, frontPos["phoneValue"], 25, false, SKColors.Black);

                DrawText(canvas, "Correo Electrónico", frontPos["emailLabel"], 26, true, blue);
                DrawText(canvas, userData.Email, frontPos["emailValue"], 25, false, SKColors.Black);

                DrawText(canvas, $"RH: {userData.BloodTypeValue}", frontPos["bloodTypeValue"], 26, true, blue);

                // ID
                DrawText(canvas, $"ID: {userData.CardId}", frontPos["cardId"], 26, true, SKColors.Blue);

                pdf.EndPage();
            }

            // ========================= BACK =========================
            using (var canvas = pdf.BeginPage(pageW, pageH))
            {
                canvas.Clear(SKColors.White);

                // Fondo al 100%
                var pic = backSvg.Picture;
                if (pic != null)
                {
                    var bounds = pic.CullRect;
                    canvas.Save();
                    canvas.Scale(pageW / bounds.Width, pageH / bounds.Height);
                    canvas.DrawPicture(pic);
                    canvas.Restore();
                }

                canvas.Scale(globalScale);
                var backPos = GetBackPositions();

                // “Términos” (línea 1)  /  “y Condiciones” (línea 2) – un poco más grande
                DrawText(canvas, "Términos", backPos["titleTop"], 42, true, new SKColor(30, 30, 30));
                DrawText(canvas, "y Condiciones", backPos["titleBottom"], 42, true, new SKColor(30, 30, 30));

                // Bullets más angostos (no se salen)
                var bulletsMaxWidth = 360f;
                DrawWrappedText(canvas, "• Este carnet debe presentarse al ingresar.", backPos["guides"], 30, false, SKColors.Black, bulletsMaxWidth, 30);
                DrawWrappedText(canvas, "• Cualquier alteración o mal uso será sancionado.", backPos["guides2"], 30, false, SKColors.Black, bulletsMaxWidth, 30);

                // Datos de contacto (igual que antes)
                DrawText(canvas, $"Email: {userData.BranchEmail}", backPos["branchEmail"], 28, false, new SKColor(50, 50, 50));
                DrawText(canvas, $"Dirección: {userData.BranchAddress}", backPos["branchAddress"], 28, false, new SKColor(50, 50, 50));
                DrawText(canvas, $"Teléfono: {userData.BranchPhone}", backPos["branchPhone"], 28, false, new SKColor(50, 50, 50));
                DrawText(canvas, $"Emitido: {DateTime.Now:dd/MM/yyyy}", backPos["issuedDate"], 28, false, SKColors.Gray);

                pdf.EndPage();
            }

            pdf.Close();
        }

        // -------------------- Posiciones Frente --------------------
        private Dictionary<string, Position> GetFrontPositions() => new()
        {
            ["logo"] = new Position(55, 140, 100, 60),
            ["companyName"] = new Position(180, 190),
            ["qr"] = new Position(400, 110, 150, 150),
            ["userPhoto"] = new Position(80, 320, 205, 280),
            ["profile"] = new Position(440, 290), // bajo el QR
            ["name"] = new Position(320, 430),
            ["identification"] = new Position(320, 480),
            ["internalDivision"] = new Position(320, 520),
            ["categoryArea"] = new Position(220, 640), // centrada entre foto y datos
            ["phoneLabel"] = new Position(120, 760),
            ["phoneValue"] = new Position(120, 790),
            ["emailLabel"] = new Position(120, 830),
            ["emailValue"] = new Position(120, 860),
            ["bloodTypeValue"] = new Position(520, 790),
            ["cardId"] = new Position(200, 910)
        };

        // -------------------- Posiciones Reverso --------------------
        private Dictionary<string, Position> GetBackPositions() => new()
        {
            ["titleTop"] = new Position(160, 340),
            ["titleBottom"] = new Position(120, 380),

            // bullets angostos (ocupan más alto y menos ancho)
            ["guides"] = new Position(100, 520),
            ["guides2"] = new Position(100, 560),

            ["branchEmail"] = new Position(100, 820),
            ["branchAddress"] = new Position(100, 860),
            ["branchPhone"] = new Position(100, 900),
            ["issuedDate"] = new Position(100, 960)
        };

        // -------------------- Helpers de dibujo --------------------
        private sealed class Position
        {
            public float x { get; }
            public float y { get; }
            public float? width { get; }
            public float? height { get; }
            public Position(float x, float y, float? width = null, float? height = null)
            { this.x = x; this.y = y; this.width = width; this.height = height; }
        }

        private void DrawShadowedText(SKCanvas canvas, string? text, Position pos, float size, bool bold = false, SKColor? color = null)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            using var shadow = new SKPaint
            {
                Color = new SKColor(0, 0, 0, 80),
                TextSize = size,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial", bold ? SKFontStyle.Bold : SKFontStyle.Normal)
            };
            canvas.DrawText(text, pos.x + 2, pos.y + 2, shadow);

            using var paint = new SKPaint
            {
                Color = color ?? SKColors.Black,
                TextSize = size,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial", bold ? SKFontStyle.Bold : SKFontStyle.Normal)
            };
            canvas.DrawText(text, pos.x, pos.y, paint);
        }

        private void DrawMultilineText(SKCanvas canvas, string? text, Position pos, float size,
                                       bool bold = false, SKColor? color = null,
                                       float maxWidth = 0, float lineSpacing = 32)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            using var paint = new SKPaint
            {
                Color = color ?? SKColors.Black,
                TextSize = size,
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High,
                Typeface = SKFontManager.Default.MatchFamily("Arial", bold ? SKFontStyle.Bold : SKFontStyle.Normal)
            };

            var words = text.Split(' ');
            float x = pos.x, y = pos.y;
            string line = "";

            foreach (var w in words)
            {
                var test = string.IsNullOrEmpty(line) ? w : $"{line} {w}";
                var width = paint.MeasureText(test);
                if (maxWidth > 0 && width > maxWidth)
                {
                    canvas.DrawText(line, x, y, paint);
                    line = w;
                    y += lineSpacing;
                }
                else line = test;
            }
            if (!string.IsNullOrWhiteSpace(line)) canvas.DrawText(line, x, y, paint);
        }

        private void DrawWrappedText(SKCanvas canvas, string text, Position pos, float size,
                                     bool bold, SKColor color, float maxWidth, float lineSpacing)
        {
            DrawMultilineText(canvas, text, pos, size, bold, color, maxWidth, lineSpacing);
        }

        private async Task<SKImage?> LoadImageOrFallbackAsync(string? url, string fallbackText)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    var bytes = await _http.GetByteArrayAsync(url);
                    using var data = SKData.CreateCopy(bytes);
                    return SKImage.FromEncodedData(data);
                }
            }
            catch { /* fallback */ }

            using var surface = SKSurface.Create(new SKImageInfo(120, 120));
            var c = surface.Canvas;
            c.Clear(new SKColor(220, 220, 220));
            using var paint = new SKPaint { Color = SKColors.Gray, TextAlign = SKTextAlign.Center, TextSize = 18, IsAntialias = true };
            c.DrawText(fallbackText, 60, 65, paint);
            return surface.Snapshot();
        }

        private SKImage? DecodeBase64Image(string? base64)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(base64)) return null;
                var bytes = Convert.FromBase64String(base64);
                using var data = SKData.CreateCopy(bytes);
                return SKImage.FromEncodedData(data);
            }
            catch { return null; }
        }

        private void DrawImage(SKCanvas canvas, SKImage? img, Position pos)
        {
            if (img == null) return;
            var rect = new SKRect(pos.x, pos.y, pos.x + (pos.width ?? img.Width), pos.y + (pos.height ?? img.Height));
            canvas.DrawImage(img, rect, new SKPaint { FilterQuality = SKFilterQuality.High, IsAntialias = true });
        }

        private void DrawText(SKCanvas canvas, string? text, Position pos, float size, bool bold = false, SKColor? color = null)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            using var paint = new SKPaint
            {
                Color = color ?? SKColors.Black,
                TextSize = size,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial", bold ? SKFontStyle.Bold : SKFontStyle.Normal)
            };
            canvas.DrawText(text, pos.x, pos.y, paint);
        }

        private static float MmToPt(float mm) => mm * 72f / 25.4f;
    }
}

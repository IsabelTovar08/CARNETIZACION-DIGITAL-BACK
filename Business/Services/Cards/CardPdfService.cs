using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Entity.DTOs.Operational.Response;
using Entity.DTOs.Specifics.Cards;
using SkiaSharp;
using Svg.Skia;

namespace Business.Services.Cards
{
    /// <summary>
    /// Genera carnet PDF (vertical 85.6x54 mm) con SVG al 100% del tamaño del PDF.
    /// Todas las fuentes se auto-ajustan al espacio disponible.
    /// </summary>
    public class CardPdfService : ICardPdfService
    {
        private readonly HttpClient _http;

        public CardPdfService(HttpClient httpClient) => _http = httpClient;

        /// <summary>
        /// Genera el PDF del carnet a partir de una plantilla y datos de usuario.
        /// </summary>
        public async Task GenerateCardAsync(CardTemplateResponse template, CardUserData userData, Stream output)
        {
            // Tamaño físico del carnet (mm a puntos)
            float pageW = MmToPt(54f);   // ~153 pt
            float pageH = MmToPt(85.6f); // ~243 pt

            // Base de diseño de posiciones (coordenadas lógicas)
            float designWidth = 640f;
            float designHeight = 1010f;

            // Cargar fondos SVG
            var frontSvgBytes = await _http.GetByteArrayAsync(template.FrontBackgroundUrl);
            var backSvgBytes = await _http.GetByteArrayAsync(template.BackBackgroundUrl);

            using var frontSvg = new SKSvg();
            using var backSvg = new SKSvg();
            using (var fs = new MemoryStream(frontSvgBytes)) frontSvg.Load(fs);
            using (var bs = new MemoryStream(backSvgBytes)) backSvg.Load(bs);

            // Imágenes
            using var logoImg = DecodeBase64Image(userData.LogoUrl);
            using var userPhoto = await LoadImageOrFallbackAsync(userData.UserPhotoUrl, "Sin Foto");

            using var qrImgRaw = DecodeBase64Image(userData.QrUrl);
            using var qrImg = qrImgRaw != null ? CropQr(qrImgRaw) : null;

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
                var frontPic = frontSvg.Picture;
                if (frontPic != null)
                {
                    var bounds = frontPic.CullRect;
                    canvas.Save();
                    canvas.Scale(pageW / bounds.Width, pageH / bounds.Height);
                    canvas.DrawPicture(frontPic);
                    canvas.Restore();
                }

                canvas.Scale(globalScale);
                var frontPos = GetFrontPositions();

                // Logo
                DrawImage(canvas, logoImg, frontPos["logo"], preserveSharpness: false);

                // Nombre institución (2 líneas, auto-ajustable)
                DrawMultilineTextAutoSize(
                    canvas,
                    userData.CompanyName?.ToUpper(),
                    frontPos["companyName"],
                    maxWidth: 220,
                    maxHeight: 70,
                    initialSize: 28,
                    minSize: 16,
                    bold: true,
                    color: SKColors.White
                );

                // Foto
                DrawImage(canvas, userPhoto, frontPos["userPhoto"], preserveSharpness: false);

                // QR (auto recortado y nítido)
                DrawImage(canvas, qrImg, frontPos["qr"], preserveSharpness: true);

                // Nombre (nombres arriba, apellidos abajo, auto-ajuste)
                DrawNameWithAutoSize(canvas, userData.Name, frontPos["name"], maxWidth: 200f);

                // Identificación (CC)
                DrawTextAutoSize(
                    canvas,
                    $"{userData.DocumentName ?? "CC"}: {userData.DocumentNumber}",
                    frontPos["identification"],
                    maxWidth: 260,
                    maxHeight: 40,
                    initialSize: 30,
                    minSize: 16,
                    bold: true,
                    color: new SKColor(40, 40, 40)
                );

                // División interna
                DrawTextAutoSize(
                    canvas,
                    userData.InternalDivisionName,
                    frontPos["internalDivision"],
                    maxWidth: 260,
                    maxHeight: 40,
                    initialSize: 30,
                    minSize: 14,
                    bold: false,
                    color: SKColors.DarkSlateGray
                );

                // Unidad organizativa (centrada entre foto y datos)
                DrawTextAutoSize(
                    canvas,
                    userData.OrganizationalUnit,
                    frontPos["categoryArea"],
                    maxWidth: 380,
                    maxHeight: 45,
                    initialSize: 26,
                    minSize: 14,
                    bold: false,
                    color: new SKColor(50, 50, 50)
                );

                // Perfil (debajo del QR)
                DrawTextAutoSize(
                    canvas,
                    userData.Profile?.ToUpper(),
                    frontPos["profile"],
                    maxWidth: 200,
                    maxHeight: 40,
                    initialSize: 28,
                    minSize: 14,
                    bold: true,
                    color: new SKColor(60, 60, 60)
                );

                var blue = new SKColor(0, 80, 160);

                // Teléfono label
                DrawTextAutoSize(
                    canvas,
                    "Teléfono",
                    frontPos["phoneLabel"],
                    maxWidth: 200,
                    maxHeight: 30,
                    initialSize: 26,
                    minSize: 14,
                    bold: true,
                    color: blue
                );

                // Teléfono value
                DrawTextAutoSize(
                    canvas,
                    userData.PhoneNumber,
                    frontPos["phoneValue"],
                    maxWidth: 260,
                    maxHeight: 35,
                    initialSize: 25,
                    minSize: 14,
                    bold: false,
                    color: SKColors.Black
                );

                // Correo label
                DrawTextAutoSize(
                    canvas,
                    "Correo Electrónico",
                    frontPos["emailLabel"],
                    maxWidth: 260,
                    maxHeight: 35,
                    initialSize: 26,
                    minSize: 14,
                    bold: true,
                    color: blue
                );

                // Correo value
                DrawTextAutoSize(
                    canvas,
                    userData.Email,
                    frontPos["emailValue"],
                    maxWidth: 380,
                    maxHeight: 40,
                    initialSize: 25,
                    minSize: 12,
                    bold: false,
                    color: SKColors.Black
                );

                // RH
                DrawTextAutoSize(
                    canvas,
                    $"RH: {userData.BloodTypeValue}",
                    frontPos["bloodTypeValue"],
                    maxWidth: 140,
                    maxHeight: 30,
                    initialSize: 26,
                    minSize: 14,
                    bold: true,
                    color: blue
                );

                // Área
                DrawTextAutoSize(
                    canvas,
                    $"Área: {userData.CategoryArea}",
                    frontPos["area"],
                    maxWidth: 380,
                    maxHeight: 45,
                    initialSize: 26,
                    minSize: 12,
                    bold: true,
                    color: SKColors.Blue
                );

                pdf.EndPage();
            }

            // ========================= BACK =========================
            using (var canvas = pdf.BeginPage(pageW, pageH))
            {
                canvas.Clear(SKColors.White);

                // Fondo al 100%
                var backPic = backSvg.Picture;
                if (backPic != null)
                {
                    var bounds = backPic.CullRect;
                    canvas.Save();
                    canvas.Scale(pageW / bounds.Width, pageH / bounds.Height);
                    canvas.DrawPicture(backPic);
                    canvas.Restore();
                }

                canvas.Scale(globalScale);
                var backPos = GetBackPositions();

                // Títulos "Términos" / "y Condiciones"
                DrawTextAutoSize(
                    canvas,
                    "Términos",
                    backPos["titleTop"],
                    maxWidth: 480,     
                    maxHeight: 70,    
                    initialSize: 60,   
                    minSize: 38,      
                    bold: true,
                    color: new SKColor(30, 30, 30)
                );



                DrawTextAutoSize(
                    canvas,
                    "y Condiciones",
                    backPos["titleBottom"],
                    maxWidth: 520,     
                    maxHeight: 70,     
                    initialSize: 58,   
                    minSize: 34,       
                    bold: true,
                    color: new SKColor(30, 30, 30)
                );


                // Bullets (se mantienen con ajuste simple de párrafo)
                var bulletsMaxWidth = 480f;
                DrawWrappedText(canvas,
                    "• Este carnet debe presentarse al ingresar.",
                    backPos["guides"],
                    size: 30,
                    bold: false,
                    color: SKColors.Black,
                    maxWidth: bulletsMaxWidth,
                    lineSpacing: 30);

                DrawWrappedText(canvas,
                    "• Cualquier alteración o mal uso será sancionado.",
                    backPos["guides2"],
                    size: 30,
                    bold: false,
                    color: SKColors.Black,
                    maxWidth: bulletsMaxWidth,
                    lineSpacing: 30);

                // Email sucursal
                DrawTextAutoSize(
                    canvas,
                    $"Email: {userData.BranchEmail}",
                    backPos["branchEmail"],
                    maxWidth: 420,
                    maxHeight: 40,
                    initialSize: 28,
                    minSize: 14,
                    bold: false,
                    color: new SKColor(50, 50, 50)
                );

                // Dirección sucursal
                DrawTextAutoSize(
                    canvas,
                    $"Dirección: {userData.BranchAddress}",
                    backPos["branchAddress"],
                    maxWidth: 420,
                    maxHeight: 40,
                    initialSize: 28,
                    minSize: 14,
                    bold: false,
                    color: new SKColor(50, 50, 50)
                );

                // Teléfono sucursal
                DrawTextAutoSize(
                    canvas,
                    $"Teléfono: {userData.BranchPhone}",
                    backPos["branchPhone"],
                    maxWidth: 420,
                    maxHeight: 40,
                    initialSize: 28,
                    minSize: 14,
                    bold: false,
                    color: new SKColor(50, 50, 50)
                );

                // Válido desde
                DrawTextAutoSize(
                    canvas,
                    $"Válido desde: {userData.ValidFrom:dd/MM/yyyy}",
                    backPos["validFrom"],
                    maxWidth: 240,
                    maxHeight: 40,
                    initialSize: 20,
                    minSize: 16,
                    bold: false,
                    color: new SKColor(40, 40, 40)
                );

                // Válido hasta
                DrawTextAutoSize(
                    canvas,
                    $"Válido hasta: {userData.ValidUntil:dd/MM/yyyy}",
                    backPos["validUntil"],
                    maxWidth: 240,
                    maxHeight: 40,
                    initialSize: 20,
                    minSize: 16,
                    bold: false,
                    color: new SKColor(40, 40, 40)
                );

                // =====================
                // COLUMNA DERECHA
                // =====================

                // Emitido
                DrawTextAutoSize(
                    canvas,
                    $"Emitido: {userData.IssuedDate:dd/MM/yyyy}",
                    backPos["issuedDate"],
                    maxWidth: 240,
                    maxHeight: 40,
                    initialSize: 20,
                    minSize: 16,
                    bold: false,
                    color: new SKColor(80, 80, 80)
                );

                // Descargado
                DrawTextAutoSize(
                    canvas,
                    $"Descargado: {DateTime.Now:dd/MM/yyyy}",
                    backPos["downloadedDateBack"],
                    maxWidth: 240,
                    maxHeight: 40,
                    initialSize: 20,
                    minSize: 16,
                    bold: false,
                    color: new SKColor(80, 80, 80)
                );

                pdf.EndPage();
            }

            pdf.Close();
        }

        // -------------------- Posiciones Frente --------------------
        /// <summary>
        /// Coordenadas del frente del carnet.
        /// </summary>
        private Dictionary<string, Position> GetFrontPositions() => new()
        {
            ["logo"] = new Position(55, 140, 100, 100),
            ["companyName"] = new Position(180, 190),
            ["qr"] = new Position(430, 110, 120, 120),
            ["userPhoto"] = new Position(80, 320, 205, 280),
            ["profile"] = new Position(410, 310),
            ["name"] = new Position(320, 430),
            ["identification"] = new Position(320, 510),
            ["internalDivision"] = new Position(320, 550),
            ["categoryArea"] = new Position(220, 640),
            ["phoneLabel"] = new Position(120, 760),
            ["phoneValue"] = new Position(120, 790),
            ["emailLabel"] = new Position(120, 830),
            ["emailValue"] = new Position(120, 860),
            ["bloodTypeValue"] = new Position(520, 790),
            ["area"] = new Position(320, 910)
        };

        // -------------------- Posiciones Reverso --------------------
        /// <summary>
        /// Coordenadas del reverso del carnet.
        /// </summary>
        private Dictionary<string, Position> GetBackPositions() => new()
        {
            ["titleTop"] = new Position(175, 350),
            ["titleBottom"] = new Position(135, 400),
            ["guides"] = new Position(100, 550),
            ["guides2"] = new Position(100, 650),
            ["branchEmail"] = new Position(100, 810),
            ["branchAddress"] = new Position(100, 850),
            ["branchPhone"] = new Position(100, 890),

            // ======= COLUMNA IZQUIERDA (VÁLIDEZ) =======
            ["validFrom"] = new Position(100, 950),
            ["validUntil"] = new Position(100, 990),

            // ======= COLUMNA DERECHA (EMITIDO / DESCARGADO) =======
            ["issuedDate"] = new Position(360, 950),
            ["downloadedDateBack"] = new Position(360, 990)

        };

        // -------------------- Helpers de dibujo --------------------

        /// <summary>
        /// Modelo para representar posición y tamaño opcional.
        /// </summary>
        private sealed class Position
        {
            public float x { get; }
            public float y { get; }
            public float? width { get; }
            public float? height { get; }
            public Position(float x, float y, float? width = null, float? height = null)
            {
                this.x = x;
                this.y = y;
                this.width = width;
                this.height = height;
            }
        }

        /// <summary>
        /// Dibuja texto con sombra suave.
        /// </summary>
        private void DrawShadowedText(SKCanvas canvas, string? text, Position pos, float size, bool bold = false, SKColor? color = null)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            using var shadow = new SKPaint
            {
                Color = new SKColor(0, 0, 0, 80),
                TextSize = size,
                IsAntialias = true,
                Typeface = SKFontManager.Default.MatchFamily("Arial",
                    bold ? SKFontStyle.Bold : SKFontStyle.Normal)
            };
            canvas.DrawText(text, pos.x + 2, pos.y + 2, shadow);

            using var paint = new SKPaint
            {
                Color = color ?? SKColors.Black,
                TextSize = size,
                IsAntialias = true,
                Typeface = SKFontManager.Default.MatchFamily("Arial",
                    bold ? SKFontStyle.Bold : SKFontStyle.Normal)
            };
            canvas.DrawText(text, pos.x, pos.y, paint);
        }

        /// <summary>
        /// Dibuja texto auto-ajustando tamaño para una sola línea.
        /// </summary>
        private void DrawTextAutoSize(
            SKCanvas canvas,
            string? text,
            Position pos,
            float maxWidth,
            float maxHeight,
            float initialSize,
            float minSize,
            bool bold,
            SKColor? color = null)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            using var paint = new SKPaint
            {
                Color = color ?? SKColors.Black,
                IsAntialias = true,
                Typeface = SKFontManager.Default.MatchFamily("Arial",
                    bold ? SKFontStyle.Bold : SKFontStyle.Normal)
            };

            float size = initialSize;

            while (size >= minSize)
            {
                paint.TextSize = size;

                float width = paint.MeasureText(text);
                float height = paint.FontMetrics.Descent - paint.FontMetrics.Ascent;

                if (width <= maxWidth && height <= maxHeight)
                    break;

                size -= 1.5f;
            }

            paint.TextSize = size;
            canvas.DrawText(text, pos.x, pos.y, paint);
        }

        /// <summary>
        /// Dibuja texto en máximo dos líneas, auto-ajustando el tamaño.
        /// Pensado para textos tipo "UNIVERSIDAD NACIONAL".
        /// </summary>
        private void DrawMultilineTextAutoSize(
            SKCanvas canvas,
            string? text,
            Position pos,
            float maxWidth,
            float maxHeight,
            float initialSize,
            float minSize,
            bool bold,
            SKColor? color = null,
            float lineSpacing = 30f)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0) return;

            using var paint = new SKPaint
            {
                Color = color ?? SKColors.Black,
                IsAntialias = true,
                Typeface = SKFontManager.Default.MatchFamily("Arial",
                    bold ? SKFontStyle.Bold : SKFontStyle.Normal)
            };

            float size = initialSize;

            while (size >= minSize)
            {
                paint.TextSize = size;

                var lines = new List<string>();
                string current = string.Empty;

                foreach (var w in words)
                {
                    var test = string.IsNullOrEmpty(current) ? w : $"{current} {w}";
                    float widthTest = paint.MeasureText(test);
                    if (widthTest > maxWidth && !string.IsNullOrEmpty(current))
                    {
                        lines.Add(current);
                        current = w;
                    }
                    else
                    {
                        current = test;
                    }
                }

                if (!string.IsNullOrEmpty(current))
                    lines.Add(current);

                float totalHeight = lines.Count * lineSpacing;

                if (lines.Count <= 2 && totalHeight <= maxHeight)
                {
                    // Dibuja las líneas calculadas
                    float x = pos.x;
                    float y = pos.y;
                    foreach (var line in lines)
                    {
                        canvas.DrawText(line, x, y, paint);
                        y += lineSpacing;
                    }
                    return;
                }

                size -= 1.5f;
            }

            // Si llega aquí usa el tamaño mínimo
            paint.TextSize = minSize;
            canvas.DrawText(text, pos.x, pos.y, paint);
        }

        /// <summary>
        /// Dibuja texto envuelto en múltiples líneas sin auto-ajuste de tamaño.
        /// </summary>
        private void DrawWrappedText(SKCanvas canvas, string text, Position pos, float size,
                                     bool bold, SKColor color, float maxWidth, float lineSpacing)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            using var paint = new SKPaint
            {
                Color = color,
                TextSize = size,
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High,
                Typeface = SKFontManager.Default.MatchFamily("Arial",
                    bold ? SKFontStyle.Bold : SKFontStyle.Normal)
            };

            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            float x = pos.x;
            float y = pos.y;
            string line = string.Empty;

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
                else
                {
                    line = test;
                }
            }

            if (!string.IsNullOrWhiteSpace(line))
                canvas.DrawText(line, x, y, paint);
        }

        /// <summary>
        /// Carga una imagen desde URL o dibuja un placeholder con texto.
        /// </summary>
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
            catch
            {
                // Usa fallback
            }

            using var surface = SKSurface.Create(new SKImageInfo(120, 120));
            var c = surface.Canvas;
            c.Clear(new SKColor(220, 220, 220));
            using var paint = new SKPaint
            {
                Color = SKColors.Gray,
                TextAlign = SKTextAlign.Center,
                TextSize = 18,
                IsAntialias = true
            };
            c.DrawText(fallbackText, 60, 65, paint);
            return surface.Snapshot();
        }

        /// <summary>
        /// Decodifica una imagen en base64 (acepta prefijos tipo data:image/png;base64,).
        /// </summary>
        private SKImage? DecodeBase64Image(string? base64)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(base64)) return null;

                // Limpia prefijos como "data:image/png;base64,"
                var clean = base64.Contains("base64,", StringComparison.OrdinalIgnoreCase)
                    ? base64[(base64.IndexOf("base64,", StringComparison.OrdinalIgnoreCase) + 7)..]
                    : base64;

                var bytes = Convert.FromBase64String(clean);
                using var data = SKData.CreateCopy(bytes);
                return SKImage.FromEncodedData(data);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Dibuja una imagen en el canvas con opción para conservar nitidez (útil para QR).
        /// </summary>
        private void DrawImage(SKCanvas canvas, SKImage? img, Position pos, bool preserveSharpness)
        {
            if (img == null) return;

            var rect = new SKRect(
                pos.x,
                pos.y,
                pos.x + (pos.width ?? img.Width),
                pos.y + (pos.height ?? img.Height));

            using var paint = new SKPaint
            {
                FilterQuality = preserveSharpness ? SKFilterQuality.None : SKFilterQuality.High,
                IsAntialias = !preserveSharpness
            };

            canvas.DrawImage(img, rect, paint);
        }

        /// <summary>
        /// Convierte milímetros a puntos.
        /// </summary>
        private static float MmToPt(float mm) => mm * 72f / 25.4f;

        /// <summary>
        /// Dibuja el nombre dividido en dos líneas (nombres arriba, apellidos abajo) con auto-ajuste.
        /// </summary>
        private void DrawNameWithAutoSize(SKCanvas canvas, string? fullName, Position pos, float maxWidth)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return;

            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                DrawTextAutoSize(canvas, parts[0].ToUpper(), pos, maxWidth, maxHeight: 40, initialSize: 38, minSize: 18, bold: true, color: SKColors.Black);
                return;
            }

            string nombres = string.Join(" ", parts[..(parts.Length - 1)]).ToUpper();
            string apellidos = parts[^1].ToUpper();

            float fontSize = 40f;
            float minSize = 22f;
            float lineSpacing = 40f;

            using var paint = new SKPaint
            {
                Typeface = SKFontManager.Default.MatchFamily("Arial", SKFontStyle.Bold),
                IsAntialias = true,
                Color = SKColors.Black
            };

            // Reducir tamaño hasta que ambas líneas quepan en maxWidth
            while (fontSize >= minSize)
            {
                paint.TextSize = fontSize;

                float w1 = paint.MeasureText(nombres);
                float w2 = paint.MeasureText(apellidos);

                if (w1 <= maxWidth && w2 <= maxWidth)
                    break;

                fontSize -= 2f;
            }

            paint.TextSize = fontSize;

            // Nombres arriba
            canvas.DrawText(nombres, pos.x, pos.y, paint);
            // Apellidos abajo
            canvas.DrawText(apellidos, pos.x, pos.y + lineSpacing, paint);
        }

        /// <summary>
        /// Recorta automáticamente los bordes blancos de un QR.
        /// </summary>
        private SKImage CropQr(SKImage original)
        {
            var bmp = SKBitmap.FromImage(original);
            int w = bmp.Width;
            int h = bmp.Height;

            int left = w, right = 0, top = h, bottom = 0;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var c = bmp.GetPixel(x, y);
                    if (c.Alpha > 0 && (c.Red < 200 || c.Green < 200 || c.Blue < 200))
                    {
                        if (x < left) left = x;
                        if (x > right) right = x;
                        if (y < top) top = y;
                        if (y > bottom) bottom = y;
                    }
                }
            }

            if (left >= right || top >= bottom)
                return original;

            var cropRect = new SKRectI(left, top, right, bottom);
            var cropped = new SKBitmap(cropRect.Width, cropRect.Height);
            bmp.ExtractSubset(cropped, cropRect);

            return SKImage.FromBitmap(cropped);
        }
    }
}
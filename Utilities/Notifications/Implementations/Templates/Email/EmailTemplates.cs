using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scriban;

namespace Utilities.Notifications.Implementations.Templates.Email
{
    public enum EmailTemplate { Welcome, ResetPassword, VerifyEmail, GenericNotification }

    public static class EmailTemplates
    {
        // después (usa la carpeta del proyecto Web)
        private static string _templatesRoot = Path.Combine(
            Directory.GetCurrentDirectory(),  // = ContentRoot del Web en dev
            "Templates", "Email"
        );

        public static void SetRoot(string absoluteRoot)
        {
            if (string.IsNullOrWhiteSpace(absoluteRoot))
                throw new ArgumentException("Templates root cannot be empty.", nameof(absoluteRoot));
            _templatesRoot = absoluteRoot;
        }

        // 2) Mapa de claves -> archivo
        private static readonly Dictionary<string, string> Map = new(StringComparer.OrdinalIgnoreCase)
        {
            ["welcome"] = "Welcome.html",
            ["reset"] = "ResetPassword.html",
            ["verify"] = "Verification.html",
            ["notify"] = "GenericNotification.html"
        };

        // Cache por “origen” (ruta de archivo o nombre de recurso)
        private static readonly ConcurrentDictionary<string, Template> _cache = new();

        // ---------- API por clave
        public static Task<string> RenderByKeyAsync(string key, IDictionary<string, object> model)
        {
            if (!Map.TryGetValue(key, out var file)) file = key;
            return RenderAsync(file, model);
        }

        // ---------- API por enum
        public static Task<string> RenderAsync(EmailTemplate template, IDictionary<string, object> model)
            => RenderAsync(template switch
            {
                EmailTemplate.Welcome => "Welcome.html",
                EmailTemplate.ResetPassword => "ResetPassword.html",
                EmailTemplate.VerifyEmail => "Verification.html",
                EmailTemplate.GenericNotification => "GenericNotification.html",
                _ => throw new ArgumentOutOfRangeException(nameof(template))
            }, model);

        // ---------- API por nombre (acepta sin extensión)
        public static async Task<string> RenderAsync(string filenameOrRelativePath, IDictionary<string, object> model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var fileName = Path.GetFileName(filenameOrRelativePath);
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Template file name cannot be empty.", nameof(filenameOrRelativePath));
            if (string.IsNullOrWhiteSpace(Path.GetExtension(fileName)))
                fileName += ".html";

            // 1) Intento por archivo físico
            var fullPath = Path.Combine(_templatesRoot, fileName);
            if (File.Exists(fullPath))
            {
                Console.WriteLine($"[EmailTemplates] File: {fullPath}");
                var tpl = _cache.GetOrAdd($"file::{fullPath}", _ => ParseTemplate(File.ReadAllText(fullPath, Encoding.UTF8)));
                return await RenderWithModelAsync(tpl, model);
            }

            // 2) Intento por Embedded Resource (termina en .<fileName>, case-insensitive)
            var asm = typeof(EmailTemplates).Assembly;
            var resources = asm.GetManifestResourceNames();
            var resourceName = resources.FirstOrDefault(r =>
                r.EndsWith("." + fileName, StringComparison.OrdinalIgnoreCase));

            if (resourceName is not null)
            {
                Console.WriteLine($"[EmailTemplates] Resource: {resourceName}");
                var tpl = _cache.GetOrAdd($"res::{resourceName}", _ =>
                {
                    using var s = asm.GetManifestResourceStream(resourceName)
                        ?? throw new FileNotFoundException($"No se pudo abrir el recurso incrustado: {resourceName}");
                    using var reader = new StreamReader(s, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
                    return ParseTemplate(reader.ReadToEnd());
                });
                return await RenderWithModelAsync(tpl, model);
            }

            // 3) Diagnóstico y error
            var diag = new StringBuilder();
            diag.AppendLine($"No se encontró la plantilla por archivo ni por recurso.");
            diag.AppendLine($"Buscado archivo: {fullPath}");
            diag.AppendLine("Recursos incrustados disponibles (últimos 10):");
            foreach (var name in resources.TakeLast(10)) diag.AppendLine(" - " + name);

            throw new FileNotFoundException(diag.ToString());
        }

        // ---------- Helpers
        private static Template ParseTemplate(string text)
        {
            var tpl = Template.Parse(text);
            if (tpl.HasErrors)
                throw new InvalidOperationException(string.Join(" | ", tpl.Messages.Select(m => m.Message)));
            return tpl;
        }

        private static async Task<string> RenderWithModelAsync(Template template, IDictionary<string, object> model)
        {
            var script = new Scriban.Runtime.ScriptObject();
            foreach (var kv in model) script.Add(kv.Key, kv.Value);

            var ctx = new Scriban.TemplateContext { MemberRenamer = m => m.Name };
            ctx.PushGlobal(script);
            return await template.RenderAsync(ctx);
        }
    }
}

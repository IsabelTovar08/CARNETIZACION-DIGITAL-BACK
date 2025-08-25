using Scriban;
using System.Collections.Concurrent;
using System.Text;

namespace Utilities.Notifications.Implementations.Templates.Email
{
    // Opcional: enum para tipado fuerte (autocompletado)
    public enum EmailTemplate
    {
        Welcome,
        ResetPassword,
        VerifyEmail,
        GenericNotification
    }

    public static class EmailTemplates
    {
        // 🔥 Ruta ABSOLUTA quemada (ajústala si cambia)
        private const string TemplatesRoot =
            @"C:\Users\USUARIO\Documents\REPOSITORIOS\CARNETIZACION-DIGITAL-BACK\Utilities\Notifications\Implementations\Templates\Email";

        // Map simple: clave -> archivo .html
        private static readonly Dictionary<string, string> Map = new(StringComparer.OrdinalIgnoreCase)
        {
            ["welcome"] = "Welcome.html",
            ["reset"] = "ResetPassword.html",
            ["verify"] = "Verification.html",
            ["notify"] = "GenericNotification.html"
        };

        // Cache de plantillas compiladas (mejor rendimiento en múltiples envíos)
        private static readonly ConcurrentDictionary<string, Template> _cache = new();

        /* ===========================
           Opción A: elegir por CLAVE
           =========================== */
        public static Task<string> RenderByKeyAsync(string key, IDictionary<string, object> model)
        {
            if (!Map.TryGetValue(key, out var file))
                throw new KeyNotFoundException($"Template key '{key}' not found. Available: {string.Join(", ", Map.Keys)}");

            return RenderAsync(file, model); // reusa tu método por archivo
        }

        /* ===========================
           Opción B: elegir por ENUM
           =========================== */
        public static Task<string> RenderAsync(EmailTemplate template, IDictionary<string, object> model)
            => RenderAsync(GetFileName(template), model);

        private static string GetFileName(EmailTemplate t) => t switch
        {
            EmailTemplate.Welcome => "Welcome.html",
            EmailTemplate.ResetPassword => "ResetPassword.html",
            EmailTemplate.VerifyEmail => "VerifyEmail.html",
            EmailTemplate.GenericNotification => "GenericNotification.html",
            _ => throw new ArgumentOutOfRangeException(nameof(t))
        };

        /* ==============================================
           Método original: elegir por NOMBRE DE ARCHIVO
           ============================================== */
        public static async Task<string> RenderAsync(string filenameOrRelativePath, IDictionary<string, object> model)
        {
            var fileName = Path.GetFileName(filenameOrRelativePath);
            var fullPath = Path.Combine(TemplatesRoot, fileName);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"No se encontró la plantilla: {fullPath}");

            // Cachea el Template.Parse para no recompilar en cada envío
            var template = _cache.GetOrAdd(fullPath, path =>
            {
                var txt = File.ReadAllText(path, Encoding.UTF8);
                var tpl = Template.Parse(txt);
                if (tpl.HasErrors)
                    throw new InvalidOperationException(string.Join(" | ", tpl.Messages.Select(m => m.Message)));
                return tpl;
            });

            var script = new Scriban.Runtime.ScriptObject();
            foreach (var kv in model) script.Add(kv.Key, kv.Value);

            var ctx = new Scriban.TemplateContext { MemberRenamer = m => m.Name };
            ctx.PushGlobal(script);

            return await template.RenderAsync(ctx);
        }
    }
}

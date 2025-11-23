using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Utilities.Helpers
{
    /// <summary>
    /// Servicio para obtener información del dispositivo y ubicación del usuario.
    /// </summary>
    public class DeviceInfoService : IDeviceInfoService
    {
        /// <summary>
        /// Obtiene la marca o modelo del dispositivo usando el User-Agent.
        /// </summary>
        public string GetDeviceModel(string? userAgent)
        {
            if (string.IsNullOrWhiteSpace(userAgent))
                return "Dispositivo desconocido";

            var ua = userAgent;

            // iPhone
            if (ua.Contains("iPhone", StringComparison.OrdinalIgnoreCase))
                return "iPhone";

            // iPad
            if (ua.Contains("iPad", StringComparison.OrdinalIgnoreCase))
                return "iPad";

            // Android (extrae modelo)
            if (ua.Contains("Android", StringComparison.OrdinalIgnoreCase))
            {
                var start = ua.IndexOf("Android", StringComparison.OrdinalIgnoreCase);
                if (start > -1)
                {
                    var info = ua[(start + 8)..].Trim();
                    var semicolon = info.IndexOf(";");

                    if (semicolon > -1)
                        return info[..semicolon].Trim();
                }

                return "Android Device";
            }

            // Windows
            if (ua.Contains("Windows", StringComparison.OrdinalIgnoreCase))
                return "Windows PC";

            // MacOS
            if (ua.Contains("Macintosh", StringComparison.OrdinalIgnoreCase))
                return "MacOS";

            // Linux
            if (ua.Contains("Linux", StringComparison.OrdinalIgnoreCase))
                return "Linux";

            return "Dispositivo desconocido";
        }

        /// <summary>
        /// Obtiene la IP real del cliente considerando Azure, proxies y headers estándar.
        /// </summary>
        public string GetRealIp(IHttpContextAccessor accessor)
        {
            var request = accessor.HttpContext?.Request;

            // 1. Azure / proxies estándar
            var ip = request?.Headers["X-Forwarded-For"].ToString();
            if (!string.IsNullOrWhiteSpace(ip))
                return ip.Split(',')[0].Trim();

            // 2. Cloudflare
            ip = request?.Headers["CF-Connecting-IP"].ToString();
            if (!string.IsNullOrWhiteSpace(ip))
                return ip;

            // 3. AWS / proxies alternativos
            ip = request?.Headers["X-Client-IP"].ToString();
            if (!string.IsNullOrWhiteSpace(ip))
                return ip;

            // 4. Fallback (probablemente no sea la IP real en Azure)
            return accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString()
                   ?? "0.0.0.0";
        }

        /// <summary>
        /// Obtiene la ubicación aproximada usando un servicio de geolocalización por IP.
        /// </summary>
        public async Task<string> GetLocationFromIpAsync(string? ip)
        {
            if (string.IsNullOrWhiteSpace(ip) || ip == "0.0.0.0")
                return "Ubicación desconocida";

            try
            {
                using var http = new HttpClient();
                var url = $"https://ipinfo.io/{ip}/json";

                var json = await http.GetStringAsync(url);
                using var doc = JsonDocument.Parse(json);

                var city = doc.RootElement.TryGetProperty("city", out var cityProp)
                    ? cityProp.GetString()
                    : null;

                var country = doc.RootElement.TryGetProperty("country", out var countryProp)
                    ? countryProp.GetString()
                    : null;

                if (!string.IsNullOrWhiteSpace(city) && !string.IsNullOrWhiteSpace(country))
                    return $"{city}, {country}";

                return "Ubicación desconocida";
            }
            catch
            {
                return "Ubicación desconocida";
            }
        }
    }
}

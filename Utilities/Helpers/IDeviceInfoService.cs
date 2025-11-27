using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Utilities.Helpers
{
    public interface IDeviceInfoService
    {
        /// <summary>
        /// Obtiene la marca o modelo del dispositivo usando el User-Agent.
        /// </summary>
        string GetDeviceModel(string? userAgent);

        /// <summary>
        /// Obtiene la IP real del cliente considerando Azure/App Service/Proxies.
        /// </summary>
        string GetRealIp(IHttpContextAccessor accessor);

        /// <summary>
        /// Obtiene la ubicación aproximada usando un servicio de geolocalización por IP.
        /// </summary>
        Task<string> GetLocationFromIpAsync(string? ip);
    }
}

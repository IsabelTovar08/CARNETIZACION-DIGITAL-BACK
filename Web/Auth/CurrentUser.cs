using System.Security.Claims;
using Business.Interfaces.Auth;

namespace Web.Auth
{
    /// <summary>
    /// Implementación para obtener datos del usuario autenticado desde el token JWT.
    /// </summary>
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _http;

        public CurrentUser(IHttpContextAccessor http) => _http = http;


        public string UserIdRaw =>
            _http.HttpContext?.User?.FindFirst("sub")?.Value ??
            _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            "unknown";
        /// <summary>
        /// Id del usuario autenticado, tomado del claim "sub" o NameIdentifier.
        /// </summary>
        public int UserId
        {
            get
            {
                return int.TryParse(UserIdRaw, out var id) ? id : 0;
            }
        }

        /// <summary>
        /// Nombre o correo del usuario autenticado.
        /// </summary>
        public string? UserName =>
            _http.HttpContext?.User?.Identity?.Name ??
            _http.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
    }
}

using System.Security.Claims;
using Business.Interfaces.Auth;

namespace Web.Auth
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _http;

        public CurrentUser(IHttpContextAccessor http) => _http = http;

        public string UserId =>
            _http.HttpContext?.User?.FindFirst("sub")?.Value ??
            _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            "unknown";

        public string? UserName =>
            _http.HttpContext?.User?.Identity?.Name ??
            _http.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
    }
}

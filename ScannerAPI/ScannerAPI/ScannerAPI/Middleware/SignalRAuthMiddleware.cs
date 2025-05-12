using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;

namespace ScannerAPI.Middleware
{
    /// <summary>
    /// Middleware que asegura que las conexiones a SignalR incluyan el token JWT.
    /// </summary>
    public class SignalRAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public SignalRAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Intercepta la solicitud y transfiere el token de query string al encabezado Authorization.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;
            if (path != null && path.Contains("/scannerHub") &&
                context.Request.Query.TryGetValue("access_token", out var token))
            {
                context.Request.Headers["Authorization"] = $"Bearer {token}";
            }

            await _next(context);
        }
    }
}

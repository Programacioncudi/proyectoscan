// File: Middleware/SignalRAuthMiddleware.cs
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

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="SignalRAuthMiddleware"/>.
        /// </summary>
        /// <param name="next">Delegado al siguiente middleware en la canalización.</param>
        public SignalRAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Intercepta la solicitud y transfiere el token de query string al encabezado Authorization si corresponde a SignalR.
        /// </summary>
        /// <param name="context">Contexto HTTP de la petición.</param>
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

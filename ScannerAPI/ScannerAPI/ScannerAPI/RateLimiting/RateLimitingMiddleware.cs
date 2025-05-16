// File: RateLimiting/RateLimitingMiddleware.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ScannerAPI.RateLimiting
{
    /// <summary>
    /// Middleware de limitación de tasa para controlar peticiones.
    /// </summary>
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RateLimitOptions _options;
        private readonly IRateLimitStore _store;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="RateLimitingMiddleware"/>.
        /// </summary>
        /// <param name="next">Delegate para el siguiente middleware en la canalización.</param>
        /// <param name="options">Opciones de configuración de rate limiting.</param>
        /// <param name="store">Almacén utilizado para contar peticiones por cliente.</param>
        public RateLimitingMiddleware(RequestDelegate next, IOptions<RateLimitOptions> options, IRateLimitStore store)
        {
            _next = next;
            _options = options.Value;
            _store = store;
        }

        /// <summary>
        /// Procesa la solicitud HTTP y aplica la limitación de tasa.
        /// </summary>
        /// <param name="context">Contexto de la solicitud HTTP.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            var key = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var counter = await _store.IncrementAsync(key, _options.WindowSeconds);
            if (counter > _options.MaxRequests)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.Response.Headers["Retry-After"] = _options.WindowSeconds.ToString();
                return;
            }

            await _next(context);
        }
    }
}

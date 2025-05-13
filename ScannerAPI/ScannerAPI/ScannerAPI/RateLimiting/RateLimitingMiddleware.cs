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

        public RateLimitingMiddleware(RequestDelegate next, IOptions<RateLimitOptions> options, IRateLimitStore store)
        {
            _next = next;
            _options = options.Value;
            _store = store;
        }

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

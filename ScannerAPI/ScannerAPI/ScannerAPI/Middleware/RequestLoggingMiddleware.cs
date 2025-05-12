using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ScannerAPI.Middleware
{
    /// <summary>
    /// Middleware para registrar todas las solicitudes HTTP entrantes con su tiempo de ejecución.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Lógica del middleware que mide y registra cada solicitud.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            
            await _next(context);
            
            stopwatch.Stop();
            _logger.LogInformation(
                "Request {Method} {Url} => {StatusCode} in {Elapsed}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}

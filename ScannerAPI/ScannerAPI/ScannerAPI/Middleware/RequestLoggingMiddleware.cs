// File: Middleware/RequestLoggingMiddleware.cs
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ScannerAPI.Middleware
{
    /// <summary>
    /// Middleware para registrar petición, respuesta y duración.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="RequestLoggingMiddleware"/>.
        /// </summary>
        /// <param name="next">Delegado al siguiente middleware en la canalización.</param>
        /// <param name="logger">Logger para registrar información de la petición.</param>
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invoca el middleware, registrando al inicio y al final de la petición junto con su duración.
        /// </summary>
        /// <param name="context">Contexto HTTP de la petición.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Request {Method} {Path} started", context.Request.Method, context.Request.Path);

            await _next(context);

            stopwatch.Stop();
            _logger.LogInformation(
                "Request {Method} {Path} completed with status {StatusCode} in {Elapsed}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}

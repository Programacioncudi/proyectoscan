using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ScannerAPI.Middleware
{
    /// <summary>
    /// Middleware que simula la verificación de conexión a SignalR.
    /// </summary>
    public class SignalRConnectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SignalRConnectionMiddleware> _logger;

        public SignalRConnectionMiddleware(RequestDelegate next, ILogger<SignalRConnectionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Verifica que la solicitud contenga una conexión activa a SignalR.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("X-SignalR-Connected"))
            {
                _logger.LogWarning("Conexión SignalR no detectada");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("SignalR no conectado.");
                return;
            }

            await _next(context);
        }
    }
}

// File: Middleware/SignalRConnectionMiddleware.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ScannerAPI.Middleware
{
    /// <summary>
    /// Middleware para registrar conexiones y desconexiones de SignalR.
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

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/hubs"))
            {
                var connectionId = context.Connection.Id;
                _logger.LogInformation("SignalR conexión iniciada: {ConnectionId}", connectionId);
                context.Response.OnCompleted(() =>
                {
                    _logger.LogInformation("SignalR conexión terminada: {ConnectionId}", connectionId);
                    return Task.CompletedTask;
                });
            }

            await _next(context);
        }
    }
}



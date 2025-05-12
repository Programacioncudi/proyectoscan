using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ScannerAPI.Security.Audit
{
    /// <summary>
    /// Middleware que registra información de auditoría de cada solicitud HTTP.
    /// </summary>
    public class AuditTrailMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditTrailMiddleware> _logger;

        public AuditTrailMiddleware(RequestDelegate next, ILogger<AuditTrailMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Registra la información de la solicitud actual antes de continuar con la cadena.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            var username = context.User?.Identity?.Name ?? "Anónimo";
            var path = context.Request.Path;
            var method = context.Request.Method;
            var time = DateTime.UtcNow;

            _logger.LogInformation("Auditoría - Usuario: {User}, Método: {Method}, Ruta: {Path}, Hora: {Time}",
                username, method, path, time);

            await _next(context);
        }
    }
}

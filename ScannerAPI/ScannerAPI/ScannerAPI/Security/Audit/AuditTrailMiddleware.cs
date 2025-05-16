// File: Security/Audit/AuditTrailMiddleware.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ScannerAPI.Security.Audit
{
    /// <summary>
    /// Middleware para auditar peticiones HTTP, registrando inicio y fin de cada solicitud.
    /// </summary>
    public class AuditTrailMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditTrailMiddleware> _logger;

        /// <summary>
        /// Crea una instancia de <see cref="AuditTrailMiddleware"/>.
        /// </summary>
        /// <param name="next">Delegate al siguiente middleware en la tubería.</param>
        /// <param name="logger">Logger para registrar eventos de auditoría.</param>
        public AuditTrailMiddleware(RequestDelegate next, ILogger<AuditTrailMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Intercepta la petición HTTP, registra un evento de inicio y fin con el usuario, método y ruta.
        /// </summary>
        /// <param name="context">Contexto de la solicitud HTTP.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User.Identity?.Name ?? "Anónimo";
            _logger.LogInformation("[AUDIT] {User} - {Method} {Path} - Inicia", user, context.Request.Method, context.Request.Path);
            await _next(context);
            _logger.LogInformation("[AUDIT] {User} - {Method} {Path} - Fin {StatusCode}", user, context.Request.Method, context.Request.Path, context.Response.StatusCode);
        }
    }
}


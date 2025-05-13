using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ScannerAPI.Security.Audit
{
    /// <summary>
    /// Middleware para auditar peticiones HTTP.
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

        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User.Identity?.Name ?? "Anónimo";
            _logger.LogInformation("[AUDIT] {User} - {Method} {Path} - Inicia", user, context.Request.Method, context.Request.Path);
            await _next(context);
            _logger.LogInformation("[AUDIT] {User} - {Method} {Path} - Fin {StatusCode}", user, context.Request.Method, context.Request.Path, context.Response.StatusCode);
        }
    }
}

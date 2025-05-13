// File: Middleware/ScannerAccessMiddleware.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ScannerAPI.Middleware
{
    /// <summary>
    /// Middleware para verificar permiso "CanScan" en operaciones de escaneo.
    /// </summary>
    public class ScannerAccessMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuthorizationService _authorization;
        private readonly ILogger<ScannerAccessMiddleware> _logger;

        public ScannerAccessMiddleware(
            RequestDelegate next,
            IAuthorizationService authorization,
            ILogger<ScannerAccessMiddleware> logger)
        {
            _next = next;
            _authorization = authorization;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Omitir autorización para endpoints anónimos
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            var authResult = await _authorization.AuthorizeAsync(context.User, null, "CanScan");
            if (!authResult.Succeeded)
            {
                _logger.LogWarning("Acceso no autorizado al escaneo para usuario {User}", context.User?.Identity?.Name);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("No tienes permiso para escanear.");
                return;
            }

            await _next(context);
        }
    }
}

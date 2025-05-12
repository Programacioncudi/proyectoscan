using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ScannerAPI.Middleware
{
    /// <summary>
    /// Middleware para validar acceso a escáneres según encabezados personalizados.
    /// </summary>
    public class ScannerAccessMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ScannerAccessMiddleware> _logger;

        public ScannerAccessMiddleware(RequestDelegate next, ILogger<ScannerAccessMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Valida encabezados requeridos para acceder al escáner.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("X-Scanner-Access"))
            {
                _logger.LogWarning("Acceso al escáner denegado: falta encabezado X-Scanner-Access");
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Acceso al escáner denegado.");
                return;
            }

            await _next(context);
        }
    }
}

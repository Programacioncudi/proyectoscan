// File: RateLimiting/ScannerRateLimiter.cs
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using System.Net;

namespace ScannerAPI.RateLimiting
{
    /// <summary>
    /// Extensión para configurar rate limiting.
    /// </summary>
    public static class ScannerRateLimiter
    {
        /// <summary>
        /// Agrega un limitador de tasa con ventana fija por IP.
        /// </summary>
        /// <param name="services">Colección de servicios de inyección.</param>
        /// <param name="options">Opciones de configuración de rate limiting.</param>
        /// <returns>La propia colección de servicios para encadenamiento.</returns>
        public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services, RateLimitOptions options)
        {
            services.AddRateLimiter(config =>
            {
                config.AddPolicy("FixedWindowIpPolicy", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        context.Connection.RemoteIpAddress ?? IPAddress.None,
                        ip => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = options.MaxRequests,
                            Window = TimeSpan.FromSeconds(options.WindowSeconds),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0,
                            AutoReplenishment = true
                        }));
            });

            return services;
        }
    }
}

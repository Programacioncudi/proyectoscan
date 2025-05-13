using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace ScannerAPI.RateLimiting
{
    /// <summary>
    /// Extensión para configurar rate limiting.
    /// </summary>
    public static class ScannerRateLimiter
    {
        public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services, RateLimitOptions options)
        {
            services.AddRateLimiter(config =>
            {
                config.AddPolicy("FixedWindowIpPolicy", context =>
                    RateLimitPartition.GetIpPolicy(context.Connection.RemoteIpAddress!, _ =>
                        new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = options.MaxRequests,
                            Window = TimeSpan.FromSeconds(options.WindowSeconds),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        }));
            });
            return services;
        }
    }
}

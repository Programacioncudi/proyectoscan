using Microsoft.Extensions.DependencyInjection;
using ScannerAPI.Infrastructure.Wrappers;
using ScannerAPI.Services;
using ScannerAPI.Utilities;

namespace ScannerAPI.Services.Factories
{
    /// <summary>
    /// Fabrica para resolver IScannerService según configuración.
    /// </summary>
    public static class ScannerFactory
    {
        public static void AddScannerWrappers(this IServiceCollection services)
        {
            services.AddTransient<IScannerWrapper, TwainWrapper>();
            services.AddTransient<IScannerWrapper, WiaWrapper>();
        }
    }
}
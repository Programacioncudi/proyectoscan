using Microsoft.Extensions.DependencyInjection;
using ScannerAPI.Infrastructure.Wrappers;
using ScannerAPI.Services;
using ScannerAPI.Services.Interfaces;
using ScannerAPI.Utilities;

namespace ScannerAPI.Services.Factories
{
    /// <summary>
    /// Fabrica para resolver IScannerService según configuración.
    /// </summary>
    public static class ScannerFactory
    {
        /// <summary>
        /// Registra los wrappers de escaneo para su uso en la aplicación.
        /// </summary>
        /// <param name="services">Colección de servicios para inyección de dependencias.</param>
        public static void AddScannerWrappers(this IServiceCollection services)
        {
            services.AddTransient<IScannerWrapper, WiaWrapper>();
        }
    }
}
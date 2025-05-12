using ScannerAPI.Infrastructure.Wrappers;
using ScannerAPI.Services.Interfaces;
using ScannerAPI.Utilities;
using Microsoft.Extensions.Logging;

namespace ScannerAPI.Services.Factories
{
    /// <summary>
    /// Fábrica de instancias de escáner según tipo y arquitectura.
    /// </summary>
    public class ScannerFactory : IScannerFactory
    {
        private readonly ILogger<ScannerFactory> _logger;
        private readonly BitnessHelper _bitnessHelper;
        private readonly ILogger<Twain32Interop> _twain32Logger;
        private readonly ILogger<Twain64Interop> _twain64Logger;
        private readonly ILogger<WiaInterop> _wiaLogger;

        public ScannerFactory(
            ILogger<ScannerFactory> logger,
            BitnessHelper bitnessHelper,
            ILogger<Twain32Interop> twain32Logger,
            ILogger<Twain64Interop> twain64Logger,
            ILogger<WiaInterop> wiaLogger)
        {
            _logger = logger;
            _bitnessHelper = bitnessHelper;
            _twain32Logger = twain32Logger;
            _twain64Logger = twain64Logger;
            _wiaLogger = wiaLogger;
        }

        /// <summary>
        /// Crea una implementación de escáner basada en el tipo especificado.
        /// </summary>
        /// <param name="scannerType">Tipo del escáner: wia, twain32, twain64</param>
        /// <returns>Implementación concreta de IScannerWrapper</returns>
        public IScannerWrapper CreateScanner(string scannerType)
        {
            return scannerType.ToLower() switch
            {
                "wia" => new WiaInterop(_wiaLogger),
                "twain32" => new Twain32Interop(_twain32Logger, _bitnessHelper, null!),
                "twain64" => new Twain64Interop(_twain64Logger, _bitnessHelper, null!),
                _ => throw new ArgumentException($"Tipo de escáner no soportado: {scannerType}")
            };
        }
    }
}

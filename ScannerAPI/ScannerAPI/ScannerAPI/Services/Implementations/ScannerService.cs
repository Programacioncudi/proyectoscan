using Interop.WIA;  // COM WIA
using ScannerAPI.Models.Scanner;
using ScannerAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ScannerAPI.Services.Implementations
{
    /// <summary>
    /// Servicio que coordina el escaneo de documentos.
    /// </summary>
    public class ScannerService : IScannerService
    {
        private readonly ILogger<ScannerService> _logger;
        private readonly IScannerFactory _scannerFactory;

        public ScannerService(ILogger<ScannerService> logger, IScannerFactory scannerFactory)
        {
            _logger = logger;
            _scannerFactory = scannerFactory;
        }

        /// <summary>
        /// Ejecuta un escaneo de documento con las opciones proporcionadas.
        /// </summary>
        /// <param name="deviceId">Identificador del dispositivo.</param>
        /// <param name="options">Opciones de escaneo.</param>
        /// <returns>Resultado del escaneo.</returns>
        public async Task<ScanResult> ScanDocumentAsync(string deviceId, ScanOptions options)
        {
            try
            {
                var scanner = _scannerFactory.CreateScanner("WIA");
                return await scanner.ScanAsync(options, null);
            }
            catch (COMException ex)
            {
                _logger.LogError(ex, "Error COM al intentar escanear con WIA.");
                throw new ScannerException("Error al escanear el documento.", ex);
            }
        }
    }
}

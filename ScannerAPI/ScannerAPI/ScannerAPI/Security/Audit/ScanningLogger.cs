using Microsoft.Extensions.Logging;
using ScannerAPI.Models.Scanner;
using System;

namespace ScannerAPI.Security.Audit
{
    /// <summary>
    /// Servicio que registra auditorías relacionadas con escaneos.
    /// </summary>
    public class ScanningLogger
    {
        private readonly ILogger<ScanningLogger> _logger;

        public ScanningLogger(ILogger<ScanningLogger> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Registra un evento de escaneo exitoso.
        /// </summary>
        public void LogSuccess(ScanResult result)
        {
            _logger.LogInformation("Escaneo exitoso. Archivo generado en: {Path}", result.OutputPath);
        }

        /// <summary>
        /// Registra un evento de escaneo fallido.
        /// </summary>
        public void LogFailure(string errorMessage)
        {
            _logger.LogWarning("Escaneo fallido: {Error}", errorMessage);
        }

        /// <summary>
        /// Registra detalles adicionales de depuración.
        /// </summary>
        public void LogDebug(string info)
        {
            _logger.LogDebug("Debug de escaneo: {Info}", info);
        }

        /// <summary>
        /// Registra errores inesperados durante el escaneo.
        /// </summary>
        public void LogException(Exception ex)
        {
            _logger.LogError(ex, "Excepción durante el proceso de escaneo.");
        }
    }
}

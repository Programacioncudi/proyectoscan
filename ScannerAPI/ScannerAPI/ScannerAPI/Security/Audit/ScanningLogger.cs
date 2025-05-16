// File: Security/Audit/ScanningLogger.cs
using Microsoft.Extensions.Logging;

namespace ScannerAPI.Security.Audit
{
    /// <summary>
    /// Logger especializado para eventos y métricas de escaneo.
    /// </summary>
    public class ScanningLogger
    {
        private readonly ILogger<ScanningLogger> _logger;

        /// <summary>
        /// Crea una instancia de <see cref="ScanningLogger"/>.
        /// </summary>
        /// <param name="logger">Logger inyectado para registrar eventos de escaneo.</param>
        public ScanningLogger(ILogger<ScanningLogger> logger) => _logger = logger;

        /// <summary>
        /// Registra el inicio de un escaneo.
        /// </summary>
        /// <param name="scanId">Identificador único del escaneo.</param>
        /// <param name="userId">Identificador del usuario que inicia el escaneo.</param>
        /// <param name="deviceId">Identificador del dispositivo utilizado.</param>
        public void LogScanStart(string scanId, string userId, string deviceId) =>
            _logger.LogInformation("Scan {ScanId} start by {User} on {Device}", scanId, userId, deviceId);

        /// <summary>
        /// Registra el progreso de un escaneo.
        /// </summary>
        /// <param name="scanId">Identificador único del escaneo.</param>
        /// <param name="percent">Porcentaje completado (0-100).</param>
        public void LogScanProgress(string scanId, int percent) =>
            _logger.LogInformation("Scan {ScanId} progress {Percent}%", scanId, percent);

        /// <summary>
        /// Registra la finalización de un escaneo exitoso.
        /// </summary>
        /// <param name="scanId">Identificador único del escaneo.</param>
        /// <param name="filePath">Ruta del archivo resultante.</param>
        public void LogScanComplete(string scanId, string filePath) =>
            _logger.LogInformation("Scan {ScanId} complete, file {Path}", scanId, filePath);

        /// <summary>
        /// Registra un error ocurrido durante el escaneo.
        /// </summary>
        /// <param name="scanId">Identificador único del escaneo.</param>
        /// <param name="error">Mensaje de error descriptivo.</param>
        public void LogScanError(string scanId, string error) =>
            _logger.LogError("Scan {ScanId} error: {Error}", scanId, error);
    }
}


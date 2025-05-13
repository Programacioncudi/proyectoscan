using Microsoft.Extensions.Logging;

namespace ScannerAPI.Security.Audit
{
    /// <summary>
    /// Logger de eventos de escaneo.
    /// </summary>
    public class ScanningLogger
    {
        private readonly ILogger<ScanningLogger> _logger;

        public ScanningLogger(ILogger<ScanningLogger> logger) => _logger = logger;

        public void LogScanStart(string scanId, string userId, string deviceId) =>
            _logger.LogInformation("Scan {ScanId} start by {User} on {Device}", scanId, userId, deviceId);

        public void LogScanProgress(string scanId, int percent) =>
            _logger.LogInformation("Scan {ScanId} progress {Percent}%", scanId, percent);

        public void LogScanComplete(string scanId, string filePath) =>
            _logger.LogInformation("Scan {ScanId} complete, file {Path}", scanId, filePath);

        public void LogScanError(string scanId, string error) =>
            _logger.LogError("Scan {ScanId} error: {Error}", scanId, error);
    }
}

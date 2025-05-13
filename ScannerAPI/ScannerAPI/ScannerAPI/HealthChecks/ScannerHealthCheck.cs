// File: HealthChecks/ScannerHealthCheck.cs
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;
using ScannerAPI.Services;

namespace ScannerAPI.HealthChecks
{
    /// <summary>
    /// Comprueba disponibilidad del escáner.
    /// </summary>
    public class ScannerHealthCheck : IHealthCheck
    {
        private readonly IScannerService _scannerService;

        public ScannerHealthCheck(IScannerService scannerService)
        {
            _scannerService = scannerService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var isAvailable = await _scannerService.IsScannerAvailableAsync(cancellationToken);
            return isAvailable
                ? HealthCheckResult.Healthy("Escáner operativo.")
                : HealthCheckResult.Unhealthy("No se detecta escáner.");
        }
    }
}

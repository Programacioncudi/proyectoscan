using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.HealthChecks
{
    /// <summary>
    /// Comprueba disponibilidad del escáner.
    /// </summary>
    public class ScannerHealthCheck : IHealthCheck
    {
        private readonly IScannerService _scannerService;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ScannerHealthCheck"/>.
        /// </summary>
        /// <param name="scannerService">Servicio que gestiona operaciones de escaneo.</param>
        public ScannerHealthCheck(IScannerService scannerService)
        {
            _scannerService = scannerService;
        }

        /// <summary>
        /// Ejecuta la comprobación de estado, verificando la disponibilidad del escáner.
        /// </summary>
        /// <param name="context">Contexto de health check proporcionado por la infraestructura de ASP.NET Core.</param>
        /// <param name="cancellationToken">Token que permite cancelar la operación.</param>
        /// <returns>
        /// <see cref="HealthCheckResult.Healthy"/> si el escáner está operativo; 
        /// <see cref="HealthCheckResult.Unhealthy"/> en caso contrario.
        /// </returns>
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

using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace ScannerAPI.HealthChecks
{
    /// <summary>
    /// Verifica si el servicio de escaneo está disponible (implementación básica).
    /// </summary>
    public class ScannerHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // Aquí podrías agregar lógica real para verificar hardware, etc.
            return Task.FromResult(HealthCheckResult.Healthy("El servicio de escaneo está operativo."));
        }
    }
}

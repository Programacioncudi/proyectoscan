// File: RateLimiting/ClientRateLimitPolicy.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ScannerAPI.RateLimiting
{
    /// <summary>
    /// Política de rate limiting basada en IP de cliente.
    /// </summary>
    public class ClientRateLimitPolicy
    {
        private readonly RateLimitOptions _options;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ClientRateLimitPolicy"/>.
        /// </summary>
        /// <param name="options">Opciones de configuración para rate limiting.</param>
        public ClientRateLimitPolicy(IOptions<RateLimitOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Indica si una ruta está excluida de la política de rate limiting.
        /// </summary>
        /// <param name="path">Ruta a verificar.</param>
        /// <returns>True si la ruta está excluida; de lo contrario, false.</returns>
        public bool IsPathExcluded(PathString path)
            => _options.ExcludedPaths.Contains(path);
    }
}

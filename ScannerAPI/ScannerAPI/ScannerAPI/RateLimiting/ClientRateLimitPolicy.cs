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

        public ClientRateLimitPolicy(IOptions<RateLimitOptions> options)
        {
            _options = options.Value;
        }

        public bool IsPathExcluded(PathString path)
            => _options.ExcludedPaths.Contains(path);
    }
}
// File: RateLimiting/MemoryRateLimitStore.cs
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ScannerAPI.RateLimiting
{
    /// <summary>
    /// Implementación en memoria de <see cref="IRateLimitStore"/>.
    /// </summary>
    public class MemoryRateLimitStore : IRateLimitStore
    {
        private readonly ConcurrentDictionary<string, (int Count, DateTime Expiry)> _counters = new();

        /// <summary>
        /// Incrementa el contador para la clave dada y retorna el total actual dentro de la ventana especificada.
        /// </summary>
        /// <param name="key">Clave única para identificar el contador.</param>
        /// <param name="windowSeconds">Duración de la ventana en segundos.</param>
        /// <returns>El número de peticiones dentro de la ventana actual.</returns>
        public Task<int> IncrementAsync(string key, int windowSeconds)
        {
            var now = DateTime.UtcNow;
            var (count, expiry) = _counters.AddOrUpdate(key,
                _ => (1, now.AddSeconds(windowSeconds)),
                (_, old) => old.Expiry < now ? (1, now.AddSeconds(windowSeconds)) : (old.Count + 1, old.Expiry));
            return Task.FromResult(count);
        }
    }
}

// File: RateLimiting/MemoryRateLimitStore.cs
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ScannerAPI.RateLimiting
{
    /// <summary>
    /// Implementación en memoria de IRateLimitStore.
    /// </summary>
    public class MemoryRateLimitStore : IRateLimitStore
    {
        private readonly ConcurrentDictionary<string, (int Count, DateTime Expiry)> _counters = new();

        public Task<int> IncrementAsync(string key, int windowSeconds)
        {
            var now = DateTime.UtcNow;
            var entry = _counters.AddOrUpdate(key,
                _ => (1, now.AddSeconds(windowSeconds)),
                (_, old) => old.Expiry < now ? (1, now.AddSeconds(windowSeconds)) : (old.Count + 1, old.Expiry));
            return Task.FromResult(entry.Count);
        }
    }
}
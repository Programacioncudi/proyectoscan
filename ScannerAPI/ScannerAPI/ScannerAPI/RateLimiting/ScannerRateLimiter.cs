using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ScannerAPI.RateLimiting
{
    /// <summary>
    /// Controla la cantidad de solicitudes permitidas por escáner por unidad de tiempo.
    /// </summary>
    public class ScannerRateLimiter
    {
        private readonly int _maxRequestsPerMinute;
        private readonly ConcurrentDictionary<string, RequestBucket> _buckets = new();

        public ScannerRateLimiter(int maxRequestsPerMinute = 60)
        {
            _maxRequestsPerMinute = maxRequestsPerMinute;
        }

        /// <summary>
        /// Verifica si el dispositivo puede continuar procesando solicitudes.
        /// </summary>
        /// <param name="deviceId">Identificador del dispositivo</param>
        public bool Allow(string deviceId)
        {
            var bucket = _buckets.GetOrAdd(deviceId, _ => new RequestBucket(_maxRequestsPerMinute));
            return bucket.Allow();
        }

        private class RequestBucket
        {
            private readonly int _limit;
            private int _count;
            private DateTime _resetTime;
            private readonly object _lock = new();

            public RequestBucket(int limit)
            {
                _limit = limit;
                _resetTime = DateTime.UtcNow.AddMinutes(1);
            }

            public bool Allow()
            {
                lock (_lock)
                {
                    var now = DateTime.UtcNow;
                    if (now >= _resetTime)
                    {
                        _count = 0;
                        _resetTime = now.AddMinutes(1);
                    }

                    if (_count < _limit)
                    {
                        _count++;
                        return true;
                    }

                    return false;
                }
            }
        }
    }
}

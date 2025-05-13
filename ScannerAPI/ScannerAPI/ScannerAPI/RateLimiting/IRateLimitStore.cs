// File: RateLimiting/IRateLimitStore.cs
using System.Threading.Tasks;

namespace ScannerAPI.RateLimiting
{
    /// <summary>
    /// Almacén para conteo de peticiones.
    /// </summary>
    public interface IRateLimitStore
    {
        /// <summary>
        /// Incrementa y devuelve el contador para la clave en la ventana.
        /// </summary>
        Task<int> IncrementAsync(string key, int windowSeconds);
    }
}
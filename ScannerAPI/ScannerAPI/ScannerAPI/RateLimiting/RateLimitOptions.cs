// File: RateLimiting/RateLimitOptions.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.RateLimiting
{
    /// <summary>
    /// Opciones de configuración para rate limiting.
    /// </summary>
    public class RateLimitOptions
    {
        /// <summary>Máximo de peticiones permitidas en la ventana.</summary>
        [Range(1, int.MaxValue)]
        public int MaxRequests { get; set; }

        /// <summary>Tamaño de la ventana en segundos.</summary>
        [Range(1, 3600)]
        public int WindowSeconds { get; set; }

        /// <summary>Rutas excluidas de limitación.</summary>
        public List<string> ExcludedPaths { get; set; } = new List<string>();
    }
}
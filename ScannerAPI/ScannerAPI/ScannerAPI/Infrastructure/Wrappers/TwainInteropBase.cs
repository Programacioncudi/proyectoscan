// File: Infrastructure/Wrappers/TwainInteropBase.cs
using System.Threading;
using System.Threading.Tasks;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Infrastructure.Wrappers
{
    /// <summary>
    /// Base para implementaciones TWAIN (32/64 bits).
    /// </summary>
    public abstract class TwainInteropBase : IScannerWrapper
    {
        /// <summary>
        /// Ejecuta el escaneo con las opciones dadas y guarda en outputPath.
        /// </summary>
        public abstract Task<ScanResult> ScanAsync(ScanOptions options, string outputPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Indica si esta implementación soporta las opciones dadas.
        /// </summary>
        public abstract bool Supports(ScanOptions options);
    }
}

using System.Threading;
using System.Threading.Tasks;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Interfaz común para wrappers de dispositivos de escaneo.
    /// </summary>
    public interface IScannerWrapper
    {
        bool Supports(ScanOptions options);
        Task<ScanResult> ScanAsync(ScanOptions options, string outputPath, CancellationToken cancellationToken = default);
    }
}
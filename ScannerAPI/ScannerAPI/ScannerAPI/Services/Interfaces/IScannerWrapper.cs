using ScannerAPI.Models.Scanner;
using System.Threading;
using System.Threading.Tasks;

namespace ScannerAPI.Services.Interfaces
{
    /// <summary>
    /// Define una interfaz común para controlar dispositivos escáner (TWAIN o WIA).
    /// </summary>
    public interface IScannerWrapper
    {
        /// <summary>
        /// Indica si este wrapper soporta las opciones dadas (TWAIN vs WIA).
        /// </summary>
        bool Supports(ScanOptions options);

        /// <summary>
        /// Realiza un escaneo con las opciones especificadas, guarda en la ruta indicada
        /// y respeta la cancelación.
        /// </summary>
        Task<ScanResult> ScanAsync(
            ScanOptions options,
            string outputPath,
            CancellationToken cancellationToken
        );
    }
}


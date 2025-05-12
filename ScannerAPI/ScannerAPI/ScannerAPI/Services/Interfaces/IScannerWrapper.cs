using ScannerAPI.Models.Scanner;
using System.Threading.Tasks;

namespace ScannerAPI.Services.Interfaces
{
    /// <summary>
    /// Define una interfaz común para controlar dispositivos escáner (TWAIN o WIA).
    /// </summary>
    public interface IScannerWrapper
    {
        /// <summary>
        /// Realiza un escaneo con las opciones especificadas y guarda en el destino indicado.
        /// </summary>
        Task<ScanResult> ScanAsync(ScanOptions options, string outputPath);
    }
}

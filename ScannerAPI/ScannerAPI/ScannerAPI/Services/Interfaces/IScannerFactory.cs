// File: Services/Interfaces/IScannerFactory.cs
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Services.Interfaces
{
    /// <summary>
    /// Factoría para crear instancias de <see cref="IScannerWrapper"/> según el tipo de escáner.
    /// </summary>
    public interface IScannerFactory
    {
        /// <summary>
        /// Crea un wrapper de escáner para el tipo especificado.
        /// </summary>
        /// <param name="scannerType">Identificador del tipo de escáner (por ejemplo, "TWAIN", "WIA").</param>
        /// <returns>Una instancia de <see cref="IScannerWrapper"/> correspondiente.</returns>
        IScannerWrapper CreateScanner(string scannerType);
    }
}

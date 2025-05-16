// File: Services/Interfaces/IScannerFactory.cs
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Services.Interfaces
{
    /// <summary>
    /// Factor�a para crear instancias de <see cref="IScannerWrapper"/> seg�n el tipo de esc�ner.
    /// </summary>
    public interface IScannerFactory
    {
        /// <summary>
        /// Crea un wrapper de esc�ner para el tipo especificado.
        /// </summary>
        /// <param name="scannerType">Identificador del tipo de esc�ner (por ejemplo, "TWAIN", "WIA").</param>
        /// <returns>Una instancia de <see cref="IScannerWrapper"/> correspondiente.</returns>
        IScannerWrapper CreateScanner(string scannerType);
    }
}

// File: Models/Scanner/DeviceType.cs
namespace ScannerAPI.Models.Scanner
{
    /// <summary>
    /// Tipos de transporte de scanner soportados.
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// Escaneo mediante protocolo TWAIN.
        /// </summary>
        Twain,

        /// <summary>
        /// Escaneo mediante interfaz WIA (Windows Image Acquisition).
        /// </summary>
        Wia
    }
}

// File: Services/Interfaces/ITwainConfig.cs
namespace ScannerAPI.Services.Interfaces
{
    /// <summary>
    /// Configuración para un escáner TWAIN.
    /// </summary>
    public interface ITwainConfig
    {
        /// <summary>
        /// Nombre del escáner TWAIN a utilizar.
        /// </summary>
        string ScannerName { get; set; }

        /// <summary>
        /// Resolución en DPI para el escaneo.
        /// </summary>
        int DPI { get; set; }

        /// <summary>
        /// Ruta de salida por defecto para las imágenes escaneadas.
        /// </summary>
        string OutputPath { get; set; }
    }
}

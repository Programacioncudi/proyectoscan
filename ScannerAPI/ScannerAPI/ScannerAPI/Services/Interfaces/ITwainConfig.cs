// File: Services/Interfaces/ITwainConfig.cs
namespace ScannerAPI.Services.Interfaces
{
    /// <summary>
    /// Configuraci�n para un esc�ner TWAIN.
    /// </summary>
    public interface ITwainConfig
    {
        /// <summary>
        /// Nombre del esc�ner TWAIN a utilizar.
        /// </summary>
        string ScannerName { get; set; }

        /// <summary>
        /// Resoluci�n en DPI para el escaneo.
        /// </summary>
        int DPI { get; set; }

        /// <summary>
        /// Ruta de salida por defecto para las im�genes escaneadas.
        /// </summary>
        string OutputPath { get; set; }
    }
}

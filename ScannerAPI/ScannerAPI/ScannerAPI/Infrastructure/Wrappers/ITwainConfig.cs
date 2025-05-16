// File: Infrastructure/Wrappers/ITwainConfig.cs
namespace ScannerAPI.Infrastructure.Wrappers
{
    /// <summary>
    /// Configuración para TWAIN (bitness, timeouts, etc.).
    /// </summary>
    public interface ITwainConfig
    {
        /// <summary>
        /// Indica si se debe usar la interfaz nativa del escáner.
        /// </summary>
        bool UseNativeUI { get; }

        /// <summary>
        /// Número máximo de páginas a escanear en una sola operación.
        /// </summary>
        int MaxPages { get; }

        // añade aquí las propiedades que necesites...
    }
}

// File: Infrastructure/Wrappers/TwainRuntimeConfig.cs
namespace ScannerAPI.Infrastructure.Wrappers
{
    /// <summary>
    /// Configuración de tiempo de ejecución para TWAIN.
    /// </summary>
    public class TwainRuntimeConfig : ITwainConfig
    {
        /// <summary>
        /// Densidad de pixels por pulgada por defecto.
        /// </summary>
        public int DefaultDpi { get; set; } = 300;

        /// <summary>
        /// Indica si se debe usar interfaz nativa.
        /// </summary>
        public bool UseNativeUI { get; set; } = true;

        /// <summary>
        /// Número máximo de páginas a escanear.
        /// </summary>
        public int MaxPages { get; set; } = 1;

        /// <summary>
        /// Habilita modo dúplex (escaneo a doble cara).
        /// </summary>
        public bool Duplex { get; set; } = false;

        /// <summary>
        /// Formato de salida por defecto.
        /// </summary>
        public string DefaultFormat { get; set; } = "JPEG";
    }
}
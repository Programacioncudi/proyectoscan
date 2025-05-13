// File: Infrastructure/Wrappers/TwainRuntimeConfig.cs
namespace ScannerAPI.Infrastructure.Wrappers
{
    /// <summary>
    /// Configuración de tiempo de ejecución para TWAIN.
    /// </summary>
    public class TwainRuntimeConfig : ITwainConfig
    {
        public int DefaultDpi { get; set; } = 300;
        public bool Duplex { get; set; } = false;
        public string DefaultFormat { get; set; } = "JPEG";
    }
}

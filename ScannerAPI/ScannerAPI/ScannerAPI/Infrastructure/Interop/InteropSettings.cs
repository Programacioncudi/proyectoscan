// File: Infrastructure/Interop/InteropSettings.cs
namespace ScannerAPI.Infrastructure.Interop
{
    /// <summary>
    /// Configuración común para interop de escaneo.
    /// </summary>
    public class InteropSettings
    {
        /// <summary>
        /// Densidad de escaneo en DPI por defecto.
        /// </summary>
        public int DefaultDpi { get; set; } = 300;

        /// <summary>
        /// Indica si el escaneo dúplex está habilitado por defecto.
        /// </summary>
        public bool DefaultDuplex { get; set; } = false;

        /// <summary>
        /// Indica si el alimentador automático está habilitado por defecto.
        /// </summary>
        public bool DefaultUseFeeder { get; set; } = true;
    }
}

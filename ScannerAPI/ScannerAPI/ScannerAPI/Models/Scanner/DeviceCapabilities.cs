namespace ScannerAPI.Models.Scanner
{
    /// <summary>
    /// Representa las capacidades que puede tener un dispositivo escáner.
    /// </summary>
    public class DeviceCapabilities
    {
        /// <summary>
        /// Resolución disponible para escaneo.
        /// </summary>
        public int ResolutionDpi { get; set; }

        /// <summary>
        /// Indica si el escáner soporta escaneo a color.
        /// </summary>
        public bool SupportsColor { get; set; }

        /// <summary>
        /// Indica si el escáner soporta escaneo dúplex (doble cara).
        /// </summary>
        public bool SupportsDuplex { get; set; }
    }
}

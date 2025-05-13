namespace ScannerAPI.Infrastructure.Interop
{
    /// <summary>
    /// Configuración común para interop de escaneo.
    /// </summary>
    public class InteropSettings
    {
        public int DefaultDpi { get; set; } = 300;
        public bool DefaultDuplex { get; set; } = false;
        public bool DefaultUseFeeder { get; set; } = true;
    }
}
using WIA;
using Microsoft.Extensions.Logging;

namespace ScannerAPI.Services.Factories
{
    /// <summary>
    /// Fabrica para instanciar dispositivos WIA.
    /// </summary>
    public static class WiaDeviceFactory
    {
        public static DeviceManager CreateDeviceManager(ILogger logger)
        {
            try
            {
                var dm = new DeviceManager();
                logger.LogInformation("WIA DeviceManager creado.");
                return dm;
            }
            catch (COMException ex)
            {
                logger.LogError(ex, "No se pudo inicializar WIA DeviceManager.");
                throw;
            }
        }
    }
}

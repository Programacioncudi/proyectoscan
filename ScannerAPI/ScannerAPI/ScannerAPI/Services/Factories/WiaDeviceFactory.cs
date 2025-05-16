// File: Services/Factories/WiaDeviceFactory.cs
using System.Runtime.InteropServices;
using Interop.WIA;
using Microsoft.Extensions.Logging;

namespace ScannerAPI.Services.Factories
{
    /// <summary>
    /// Fabrica para instanciar y configurar el <see cref="DeviceManager"/> de WIA.
    /// </summary>
    public static class WiaDeviceFactory
    {
        /// <summary>
        /// Crea un <see cref="DeviceManager"/> de WIA, registrando el resultado en el log.
        /// </summary>
        /// <param name="logger">Logger para información y errores.</param>
        /// <returns>Instancia de <see cref="DeviceManager"/>.</returns>
        /// <exception cref="COMException">Si falla la inicialización del DeviceManager.</exception>
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

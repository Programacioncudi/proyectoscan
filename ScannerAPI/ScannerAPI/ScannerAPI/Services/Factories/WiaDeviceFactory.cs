using System.Collections.Generic;
using WIA;

namespace ScannerAPI.Services.Factories
{
    /// <summary>
    /// Fábrica para listar dispositivos escáner disponibles a través de WIA.
    /// </summary>
    public class WiaDeviceFactory
    {
        /// <summary>
        /// Devuelve una lista con los nombres de los dispositivos escáner disponibles mediante WIA.
        /// </summary>
        public List<string> GetAvailableDevices()
        {
            var deviceManager = new DeviceManager();
            var result = new List<string>();

            for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
            {
                DeviceInfo info = deviceManager.DeviceInfos[i];
                if (info.Type == WiaDeviceType.ScannerDeviceType)
                {
                    result.Add(info.Properties["Name"].get_Value().ToString());
                }
            }

            return result;
        }
    }
}

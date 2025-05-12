// ScannerAPI/Interfaces/IScannerService.cs
using System.Collections.Generic;
using WIA;

namespace ScannerAPI.Interfaces
{
    public interface IScannerService
    {
        // Selección y configuración de dispositivo WIA 2.0
        void SelectWIA2Device(string deviceId = null);
        void ConfigureWIA2Scan(
            int brightness = 0,
            int contrast = 0,
            int dpi = 300,
            bool useADF = false,
            bool duplex = false,
            string colorMode = "Color",
            string paperSize = "A4");

        // Operaciones de escaneo WIA 2.0
        List<ImageFile> ScanWithWIA2(int pageCount = 1, string format = "jpeg");
        Task<List<ImageFile>> ScanWithWIA2Async(int pageCount = 1, string format = "jpeg");

        // Métodos comunes
        List<string> GetAvailableWIA2Devices();
        void SaveImages(List<ImageFile> images, string outputPath, string format = "jpeg");
    }
}
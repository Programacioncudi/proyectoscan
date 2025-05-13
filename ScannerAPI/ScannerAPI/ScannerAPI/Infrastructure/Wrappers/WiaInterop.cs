

// File: Infrastructure/Wrappers/WiaInterop.cs
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WIA;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Infrastructure.Wrappers
{
    /// <summary>
    /// Implementación WIA para escaneo via COM.
    /// </summary>
    public class WiaInterop : IScannerWrapper
    {
        public bool Supports(ScanOptions options)
        {
            var deviceManager = new DeviceManager();
            return deviceManager.DeviceInfos.Cast<DeviceInfo>().Any(d => d.DeviceID == options.DeviceId);
        }

        public async Task<ScanResult> ScanAsync(ScanOptions options, string outputPath, CancellationToken cancellationToken = default)
        {
            var manager = new DeviceManager();
            var deviceInfo = manager.DeviceInfos.Cast<DeviceInfo>()
                .FirstOrDefault(d => d.DeviceID == options.DeviceId);
            if (deviceInfo == null)
                throw new InvalidOperationException($"Dispositivo WIA {options.DeviceId} no encontrado.");

            var device = deviceInfo.Connect();
            var item = device.Items[1];
            var imageFile = (ImageFile)item.Transfer(FormatID.wiaFormatJPEG);
            var bytes = (byte[])imageFile.FileData.get_BinaryData();

            await File.WriteAllBytesAsync(outputPath, bytes, cancellationToken);

            return new ScanResult
            {
                ScanId = Guid.NewGuid().ToString(),
                FilePath = outputPath,
                Success = true
            };
        }
    }
}


using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WIA = Interop.WIA;
using ScannerAPI.Services.Interfaces;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Exceptions;

namespace ScannerAPI.Infrastructure.Wrappers
{
    /// <inheritdoc />
    public class WiaInterop : IScannerWrapper
    {
        /// <inheritdoc />
        public bool Supports(ScanOptions options)
        {
            var manager = new WIA.DeviceManager();
            return manager.DeviceInfos
                          .Cast<WIA.DeviceInfo>()
                          .Any(info => info.DeviceID == options.DeviceId);
        }

        /// <inheritdoc />
        public async Task<ScanResult> ScanAsync(
            ScanOptions options,
            string outputPath,
            CancellationToken cancellationToken = default)
        {
            var manager = new WIA.DeviceManager();
            var deviceInfo = manager.DeviceInfos
                .Cast<WIA.DeviceInfo>()
                .FirstOrDefault(d => d.DeviceID == options.DeviceId)
                ?? throw new DomainException("NoScanner", $"Dispositivo WIA {options.DeviceId} no encontrado.");

            var device = deviceInfo.Connect();
            var item = device.Items[1];
            var imageFile = (WIA.ImageFile)item.Transfer(WIA.FormatID.wiaFormatJPEG);

            var bytesObj = imageFile.FileData.get_BinaryData();
            if (bytesObj is not byte[] bytes)
                throw new DomainException("ScanError", "No se pudo convertir los datos de imagen.");

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

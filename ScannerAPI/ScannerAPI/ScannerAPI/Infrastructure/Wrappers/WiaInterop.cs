using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Interop.WIA;
using Microsoft.Extensions.Logging;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Infrastructure.Wrappers
{
    /// <summary>
    /// Implementación real de escaneo usando la API WIA (Windows Image Acquisition).
    /// </summary>
    public class WiaInterop : IScannerWrapper
    {
        private readonly ILogger<WiaInterop> _logger;
        private readonly IImageProcessor _imageProcessor;

        public WiaInterop(ILogger<WiaInterop> logger, IImageProcessor imageProcessor)
        {
            _logger = logger;
            _imageProcessor = imageProcessor;
        }

        /// <summary>
        /// Lista dispositivos WIA disponibles.
        /// </summary>
        public List<DeviceInfo> GetDevices()
        {
            var devices = new List<DeviceInfo>();
            try
            {
                var manager = new DeviceManager();

                for (int i = 1; i <= manager.DeviceInfos.Count; i++)
                {
                    var info = manager.DeviceInfos[i];
                    if (info.Type == WiaDeviceType.ScannerDeviceType)
                    {
                        devices.Add(new DeviceInfo
                        {
                            SourceName = info.DeviceID,
                            DisplayName = info.Properties["Name"].get_Value().ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo dispositivos WIA");
            }

            return devices;
        }

        /// <summary>
        /// Ejecuta un escaneo con el dispositivo WIA especificado.
        /// </summary>
        public async Task<ScanResult> ScanAsync(ScanOptions options, string? outputFolder)
        {
            try
            {
                var manager = new DeviceManager();
                Device? device = null;

                for (int i = 1; i <= manager.DeviceInfos.Count; i++)
                {
                    var info = manager.DeviceInfos[i];
                    if (info.DeviceID == options.DeviceId)
                    {
                        device = info.Connect();
                        break;
                    }
                }

                if (device == null)
                    throw new Exception("No se encontró el escáner WIA especificado");

                var item = device.Items[1];
                var imageFile = (ImageFile)item.Transfer(FormatID.wiaFormatJPEG);
                var tempPath = Path.Combine(outputFolder ?? Path.GetTempPath(), $"scan_{Guid.NewGuid()}.jpg");

                imageFile.SaveFile(tempPath);

                return new ScanResult
                {
                    Success = true,
                    OutputPath = tempPath
                };
            }
            catch (COMException ex)
            {
                _logger.LogError(ex, "Error COM durante escaneo WIA.");
                return new ScanResult { Success = false, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error general durante escaneo WIA.");
                return new ScanResult { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}

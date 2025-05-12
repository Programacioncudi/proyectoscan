using System.Runtime.InteropServices;
using Interop.WIA;
using ScannerAPI.Models.Scanner;
using Microsoft.Extensions.Logging;

namespace ScannerAPI.Utilities;

public class WiaWrapper : IScannerWrapper
{
    private readonly ILogger<WiaWrapper> _logger;

    public WiaWrapper(ILogger<WiaWrapper> logger)
    {
        _logger = logger;
    }

    public Task<DeviceInfo[]> GetDevicesAsync()
    {
        try
        {
            var deviceManager = new DeviceManagerClass(); // Cambiado a DeviceManagerClass
            var deviceInfos = new List<DeviceInfo>();
            
            foreach (var deviceInfo in deviceManager.DeviceInfos)
            {
                var di = (DeviceInfo)deviceInfo;
                deviceInfos.Add(new DeviceInfo
                {
                    Id = di.DeviceID,
                    Name = GetPropertyValue(di.Properties, "Name"),
                    IsConnected = true,
                    Type = "WIA"
                });
            }

            return Task.FromResult(deviceInfos.ToArray());
        }
        catch (COMException ex)
        {
            _logger.LogError(ex, "Error enumerating WIA devices");
            throw new ScannerException("Failed to enumerate WIA devices", ex);
        }
    }

    public async Task<ScanResult> ScanAsync(ScanOptions options, IProgress<ScanProgress> progress)
    {
        try
        {
            var deviceManager = new DeviceManagerClass(); // Cambiado a DeviceManagerClass
            var device = (Device)deviceManager.DeviceInfos[options.DeviceId].Connect(); // Cast explícito
            
            progress?.Report(new ScanProgress(10, "Device connected"));

            var item = (Item)device.Items[1]; // Cast explícito
            ConfigureItem(item, options);

            progress?.Report(new ScanProgress(30, "Starting scan..."));

            var formatId = GetFormatId(options.Format);
            var imageFile = (ImageFile)item.Transfer(formatId);

            progress?.Report(new ScanProgress(70, "Processing image..."));

            using var stream = new MemoryStream();
            imageFile.SaveFileToStream(stream);
            
            return new ScanResult
            {
                ImageData = stream.ToArray(),
                Format = options.Format,
                Metadata = new ScanMetadata
                {
                    DPI = options.DPI,
                    SizeKB = stream.Length / 1024
                }
            };
        }
        catch (COMException ex)
        {
            _logger.LogError(ex, "WIA COM Exception");
            throw new ScannerException("WIA operation failed", ex);
        }
    }

    private void ConfigureItem(Item item, ScanOptions options)
    {
        SetWiaProperty(item.Properties, WIA_PROPERTIES.WIA_IPS_CUR_INTENT, (int)2); // Color
        SetWiaProperty(item.Properties, WIA_PROPERTIES.WIA_IPS_XRES, options.DPI);
        SetWiaProperty(item.Properties, WIA_PROPERTIES.WIA_IPS_YRES, options.DPI);
    }

    private string GetPropertyValue(IProperties properties, string propertyName)
    {
        try
        {
            foreach (var prop in properties)
            {
                var p = (Property)prop;
                if (p.Name == propertyName)
                    return p.get_Value().ToString();
            }
            return string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    private string GetFormatId(string format) => format.ToUpper() switch
    {
        "JPEG" => WIA_FORMATS.WIA_FORMAT_JPEG,
        "PNG" => WIA_FORMATS.WIA_FORMAT_PNG,
        "TIFF" => WIA_FORMATS.WIA_FORMAT_TIFF,
        _ => WIA_FORMATS.WIA_FORMAT_JPEG // Default
    };

    private static void SetWiaProperty(IProperties properties, int propId, object value)
    {
        try
        {
            Property prop = properties.get_Item(ref propId);
            prop.set_Value(ref value);
        }
        catch (COMException)
        {
            // Property not supported by device
        }
    }

    public Task<DeviceCapabilities> GetDeviceCapabilitiesAsync(string deviceId)
    {
        return Task.FromResult(new DeviceCapabilities
        {
            SupportsDuplex = true,
            SupportedResolutions = new[] { 75, 150, 200, 300, 600, 1200 },
            SupportedFormats = new[] { "JPEG", "PNG", "TIFF" }
        });
    }

    private static class WIA_PROPERTIES
    {
        public const int WIA_IPS_CUR_INTENT = 6146;
        public const int WIA_IPS_XRES = 6147;
        public const int WIA_IPS_YRES = 6148;
    }

    private static class WIA_FORMATS
    {
        public const string WIA_FORMAT_JPEG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";
        public const string WIA_FORMAT_PNG = "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}";
        public const string WIA_FORMAT_TIFF = "{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}";
    }
}
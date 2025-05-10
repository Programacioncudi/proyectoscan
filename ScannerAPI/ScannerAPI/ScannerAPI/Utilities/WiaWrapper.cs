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
            var deviceManager = new DeviceManager();
            return Task.FromResult(deviceManager.DeviceInfos.Cast<Interop.WIA.DeviceInfo>()
                .Select(d => new DeviceInfo
                {
                    Id = d.DeviceID,
                    Name = d.Properties["Name"].ToString(),
                    IsConnected = true,
                    Type = "WIA"
                }).ToArray());
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
            var deviceManager = new DeviceManager();
            var device = deviceManager.DeviceInfos[options.DeviceId].Connect();
            
            progress?.Report(new ScanProgress(10, "Device connected"));

            var item = device.Items[1];
            ConfigureItem(item, options);

            progress?.Report(new ScanProgress(30, "Starting scan..."));

            var imageFile = (ImageFile)item.Transfer(GetFormatId(options.Format));

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
        SetWiaProperty(item.Properties, 6146, (int)2); // Color
        SetWiaProperty(item.Properties, 6147, options.DPI);
        SetWiaProperty(item.Properties, 6148, options.DPI);
    }

    private string GetFormatId(string format) => format.ToUpper() switch
    {
        "JPEG" => "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}",
        "PNG" => "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}",
        "TIFF" => "{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}",
        _ => "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}" // JPEG por defecto
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
}
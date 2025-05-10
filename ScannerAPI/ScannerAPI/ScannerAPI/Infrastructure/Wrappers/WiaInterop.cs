using System.Runtime.InteropServices;
using Interop.WIA;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Utilities;
using Microsoft.Extensions.Logging;

namespace ScannerAPI.Infrastructure.Wrappers;

/// <summary>
/// Wrapper para la interoperabilidad con WIA (Windows Image Acquisition)
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

    public async Task<ScanResult> ScanAsync(ScanOptions options, IProgress<ScanProgress> progress)
    {
        try
        {
            var deviceManager = new DeviceManager();
            var device = deviceManager.DeviceInfos[options.DeviceId].Connect();
            
            progress.Report(new ScanProgress(10, "Device connected"));

            var scannerItem = device.Items[1];
            ConfigureScannerItem(scannerItem, options);

            progress.Report(new ScanProgress(30, "Starting scan..."));

            var imageFile = (ImageFile)scannerItem.Transfer(GetFormatId(options.Format));

            progress.Report(new ScanProgress(70, "Processing image..."));

            var result = await ProcessImage(imageFile, options);
            
            progress.Report(new ScanProgress(100, "Scan completed"));
            return result;
        }
        catch (COMException ex)
        {
            _logger.LogError(ex, "WIA COM Exception");
            throw new ScannerException("WIA operation failed", ex);
        }
    }

    private void ConfigureScannerItem(Item scannerItem, ScanOptions options)
    {
        SetWiaProperty(scannerItem.Properties, 6146, (int)2); // WIA_IPA_DATATYPE = Color
        SetWiaProperty(scannerItem.Properties, 6147, options.DPI); // WIA_IPA_HORIZONTAL_RESOLUTION
        SetWiaProperty(scannerItem.Properties, 6148, options.DPI); // WIA_IPA_VERTICAL_RESOLUTION
        
        if (options.UseDuplex)
        {
            SetWiaProperty(scannerItem.Properties, 3088, (int)4); // WIA_IPS_DOCUMENT_HANDLING_SELECT = Duplex
        }
    }

    private async Task<ScanResult> ProcessImage(ImageFile imageFile, ScanOptions options)
    {
        using var stream = new MemoryStream();
        imageFile.SaveFileToStream(stream);
        
        var processedImage = await _imageProcessor.ProcessImageAsync(
            stream.ToArray(), 
            options.Format, 
            options.Quality);
            
        return new ScanResult
        {
            ImageData = processedImage.Data,
            Format = processedImage.Format,
            Metadata = new ScanMetadata
            {
                Width = processedImage.Width,
                Height = processedImage.Height,
                DPI = options.DPI,
                SizeKB = processedImage.Data.Length / 1024
            }
        };
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

    public Task<DeviceInfo[]> GetDevicesAsync()
    {
        var deviceManager = new DeviceManager();
        return Task.FromResult(deviceManager.DeviceInfos.Cast<DeviceInfo>()
            .Select(d => new DeviceInfo
            {
                Id = $"WIA_{d.DeviceID}",
                Name = d.Properties["Name"].ToString(),
                IsConnected = true,
                Type = "WIA"
            }).ToArray());
    }

    public Task<DeviceCapabilities> GetDeviceCapabilitiesAsync(string deviceId)
    {
        var deviceManager = new DeviceManager();
        var device = deviceManager.DeviceInfos[deviceId.Replace("WIA_", "")].Connect();
        var item = device.Items[1];

        var capabilities = new DeviceCapabilities
        {
            SupportsDuplex = HasWiaProperty(item.Properties, 3088),
            SupportedResolutions = GetWiaSupportedResolutions(item.Properties),
            SupportedFormats = new[] { "JPEG", "PNG", "TIFF" }
        };

        return Task.FromResult(capabilities);
    }

    private bool HasWiaProperty(IProperties properties, int propertyId)
    {
        try
        {
            var prop = properties.get_Item(ref propertyId);
            return prop != null;
        }
        catch
        {
            return false;
        }
    }

    private int[] GetWiaSupportedResolutions(IProperties properties)
    {
        // Implementación simplificada - en producción debería leer las resoluciones soportadas
        return new[] { 75, 150, 200, 300, 600, 1200 };
    }
}

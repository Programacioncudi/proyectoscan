using System.Runtime.InteropServices;
using Interop.WIA;
using ScannerAPI.Infrastructure.Wrappers;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Services.Factories;
using ScannerAPI.Utilities;

namespace ScannerAPI.Services.Implementations;

/// <summary>
/// Implementación del servicio de escáner que utiliza WIA 2.0 y TWAIN (a través de ntwain)
/// </summary>
public class ScannerService : IScannerService
{
    private readonly IScannerFactory _scannerFactory;
    private readonly IEventBusService _eventBus;
    private readonly ILogger<ScannerService> _logger;
    private readonly BitnessHelper _bitnessHelper;

    public ScannerService(
        IScannerFactory scannerFactory,
        IEventBusService eventBus,
        ILogger<ScannerService> logger,
        BitnessHelper bitnessHelper)
    {
        _scannerFactory = scannerFactory;
        _eventBus = eventBus;
        _logger = logger;
        _bitnessHelper = bitnessHelper;
    }

    public async Task<DeviceInfo[]> GetAvailableDevicesAsync()
    {
        try
        {
            // Primero intentamos con WIA
            var deviceManager = new DeviceManager();
            var wiaDevices = deviceManager.DeviceInfos.Cast<DeviceInfo>()
                .Select(d => new DeviceInfo
                {
                    Id = d.DeviceID,
                    Name = d.Properties["Name"].ToString(),
                    IsConnected = true,
                    Type = "WIA"
                }).ToList();

            // Luego intentamos con TWAIN si es necesario
            try
            {
                var twainWrapper = _scannerFactory.CreateScanner("twain");
                var twainDevices = await twainWrapper.GetDevicesAsync();
                wiaDevices.AddRange(twainDevices);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error enumerating TWAIN devices");
            }

            return wiaDevices.ToArray();
        }
        catch (COMException ex)
        {
            _logger.LogError(ex, "Error enumerating WIA devices");
            throw new ScannerException("Failed to enumerate devices", ex);
        }
    }

    public async Task<ScanResult> ScanDocumentAsync(ScanOptions options, string sessionId)
    {
        try
        {
            await _eventBus.PublishScanEventAsync(sessionId, "Initializing scanner...", 0);
            
            // Seleccionar la tecnología apropiada basada en el dispositivo
            var scannerTech = options.DeviceId.StartsWith("WIA") ? "wia" : 
                _bitnessHelper.Is64BitProcess ? "twain64" : "twain32";
            
            var scanner = _scannerFactory.CreateScanner(scannerTech);
            
            var result = await scanner.ScanAsync(options, new Progress<ScanProgress>(progress => 
            {
                _eventBus.PublishProgressEvent(sessionId, progress);
            }));

            await _eventBus.PublishScanEventAsync(sessionId, "Scan completed", 100);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Scan error for session {SessionId}", sessionId);
            await _eventBus.PublishScanEventAsync(sessionId, $"Scan failed: {ex.Message}", 0);
            throw new ScannerException("Scan operation failed", ex);
        }
    }

    public async Task<DeviceCapabilities> GetDeviceCapabilitiesAsync(string deviceId)
    {
        if (deviceId.StartsWith("WIA"))
        {
            return await GetWiaDeviceCapabilities(deviceId);
        }
        else
        {
            var twainWrapper = _scannerFactory.CreateScanner(
                _bitnessHelper.Is64BitProcess ? "twain64" : "twain32");
            return await twainWrapper.GetDeviceCapabilitiesAsync(deviceId);
        }
    }

    private async Task<DeviceCapabilities> GetWiaDeviceCapabilities(string deviceId)
    {
        try
        {
            var deviceManager = new DeviceManager();
            var device = deviceManager.DeviceInfos[deviceId].Connect();
            var item = device.Items[1];

            var capabilities = new DeviceCapabilities
            {
                SupportsDuplex = HasWiaProperty(item.Properties, 3088), // WIA_IPS_DOCUMENT_HANDLING_SELECT
                SupportedResolutions = GetWiaSupportedResolutions(item.Properties),
                SupportedFormats = new[] { "JPEG", "PNG", "TIFF" } // WIA soporta estos por defecto
            };

            return capabilities;
        }
        catch (COMException ex)
        {
            _logger.LogError(ex, "Error getting WIA device capabilities");
            throw new ScannerException("Failed to get device capabilities", ex);
        }
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
        // Implementación para obtener resoluciones soportadas
        return new[] { 75, 150, 200, 300, 600, 1200 };
    }
}

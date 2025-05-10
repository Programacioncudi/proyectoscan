using NTwain;
using NTwain.Data;
using ScannerAPI.Models.Scanner;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ScannerAPI.Utilities;

public class TwainWrapper : IScannerWrapper
{
    private readonly ILogger<TwainWrapper> _logger;
    private readonly ITwainConfig _config;

    public TwainWrapper(ILogger<TwainWrapper> logger, ITwainConfig config)
    {
        _logger = logger;
        _config = config;
    }

    public async Task<DeviceInfo[]> GetDevicesAsync()
    {
        try
        {
            using var session = TWainSession.Create();
            session.Open();
            return session.GetSources()
                .Select(s => new DeviceInfo
                {
                    Id = s.ProductName,
                    Name = s.ProductName,
                    IsConnected = true,
                    Type = "TWAIN"
                }).ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting TWAIN devices");
            throw new ScannerException("Failed to enumerate TWAIN devices", ex);
        }
    }

    public async Task<ScanResult> ScanAsync(ScanOptions options, IProgress<ScanProgress> progress)
    {
        using var session = TWainSession.Create();
        try
        {
            session.Open();
            var source = session.GetSources().FirstOrDefault(s => s.ProductName == options.DeviceId);
            
            if (source == null)
                throw new ScannerException("Device not found");

            session.TransferMode = _config.TransferMode;
            session.ShowUI = _config.ShowUI;
            
            // Configurar capacidades básicas
            if (session.Capabilities.ICapXResolution.IsSupported)
                session.Capabilities.ICapXResolution.SetValue(options.DPI);
            
            if (session.Capabilities.ICapYResolution.IsSupported)
                session.Capabilities.ICapYResolution.SetValue(options.DPI);

            progress?.Report(new ScanProgress(10, "Starting scan..."));
            
            var rc = session.OpenSource(source);
            if (rc != ReturnCode.Success)
                throw new ScannerException($"Failed to open source: {rc}");

            rc = session.EnableSource(_config.ShowUI ? SourceEnableMode.ShowUI : SourceEnableMode.NoUI);
            if (rc != ReturnCode.Success)
                throw new ScannerException($"Failed to enable source: {rc}");

            // Esperar a que complete el escaneo
            while (session.IsSourceEnabled)
            {
                await Task.Delay(100);
            }

            // Procesar imágenes recibidas
            return new ScanResult
            {
                ImageData = Array.Empty<byte>(), // Reemplazar con datos reales
                Format = options.Format,
                Metadata = new ScanMetadata
                {
                    DPI = options.DPI,
                    SizeKB = 0 // Reemplazar con tamaño real
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TWAIN scan error");
            throw new ScannerException("TWAIN scan failed", ex);
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

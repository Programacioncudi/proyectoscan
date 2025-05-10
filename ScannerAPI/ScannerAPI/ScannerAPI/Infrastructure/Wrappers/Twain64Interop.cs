using Microsoft.Extensions.Logging;
using NTwain;
using ScannerAPI.Models.Scanner;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScannerAPI.Infrastructure.Wrappers;

public class Twain64Interop : TwainInteropBase
{
    public Twain64Interop(ILogger<Twain64Interop> logger, BitnessHelper bitnessHelper) 
        : base(logger, bitnessHelper)
    {
    }

    public override async Task<DeviceInfo[]> GetDevicesAsync()
    {
        try
        {
            ConfigureSession();
            _session.Open();
            
            var sources = _session.GetSources().ToList();
            return sources.Select(s => new DeviceInfo
            {
                Id = s.ProductName,
                Name = s.ProductName,
                IsConnected = true,
                Type = "TWAIN64"
            }).ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting TWAIN64 devices");
            throw new ScannerException("Failed to enumerate TWAIN64 devices", ex);
        }
        finally
        {
            ReleaseResources();
        }
    }

    public override async Task<ScanResult> ScanAsync(ScanOptions options, IProgress<ScanProgress> progress)
{
    TWainSession session = null;
    try
    {
        session = TWainSession.Create();
        session.Open();
        
        ConfigureSource(session, options);
        progress?.Report(new ScanProgress(10, "Configuring scanner..."));

        // Configurar manejadores de eventos
        session.TransferReady += (s, e) => 
            progress?.Report(new ScanProgress(30, "Ready to transfer..."));
            
        var imageData = new List<byte[]>();
        session.DataTransferred += (s, e) =>
        {
            if (e.NativeData != null)
            {
                progress?.Report(new ScanProgress(60, "Transferring data..."));
                imageData.Add(e.NativeData.ToArray());
            }
        };

        // Iniciar el escaneo
        var rc = session.OpenSource(session.GetSources()
            .First(s => s.ProductName == options.DeviceId));
            
        if (rc != ReturnCode.Success)
            throw new ScannerException($"Failed to open source: {rc}");

        rc = session.EnableSource(_twainConfig.ShowUI ? 
            SourceEnableMode.ShowUI : 
            SourceEnableMode.NoUI);
            
        if (rc != ReturnCode.Success)
            throw new ScannerException($"Failed to enable source: {rc}");

        // Esperar a que complete el escaneo
        var startTime = DateTime.Now;
        while (session.IsSourceEnabled && 
               (DateTime.Now - startTime).TotalSeconds < _twainConfig.MaxWaitSeconds)
        {
            await Task.Delay(100);
        }

        if (session.IsSourceEnabled)
        {
            session.ResetSource();
            throw new ScannerException("Scan operation timed out");
        }

        // Procesar imágenes escaneadas
        progress?.Report(new ScanProgress(90, "Processing images..."));
        var processedImage = await ProcessScannedImages(imageData, options.Format);

        return new ScanResult
        {
            ImageData = processedImage,
            Format = options.Format,
            Metadata = new ScanMetadata
            {
                DPI = options.DPI,
                SizeKB = processedImage.Length / 1024
            }
        };
    }
    finally
    {
        session?.Dispose();
    }
}
    public override Task<DeviceCapabilities> GetDeviceCapabilitiesAsync(string deviceId)
    {
        // Implementación similar a GetDevicesAsync pero obteniendo capacidades
        throw new NotImplementedException();
    }
}
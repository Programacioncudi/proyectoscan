using NTwain;
using NTwain.Data;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScannerAPI.Infrastructure.Wrappers;

public abstract class TwainInteropBase : IScannerWrapper, IDisposable
{
    protected readonly ILogger _logger;
    protected readonly BitnessHelper _bitnessHelper;
    protected readonly ITwainConfig _twainConfig;
    protected TWainSession _session;
    private bool _disposed = false;

    protected TwainInteropBase(ILogger logger, BitnessHelper bitnessHelper, ITwainConfig twainConfig)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _bitnessHelper = bitnessHelper ?? throw new ArgumentNullException(nameof(bitnessHelper));
        _twainConfig = twainConfig ?? throw new ArgumentNullException(nameof(twainConfig));
    }

    protected void ConfigureSource(TWainSession session, ScanOptions options)
    {
        try
        {
            var source = session.GetSources().FirstOrDefault(s => s.ProductName == options.DeviceId);
            if (source == null)
            {
                throw new ScannerException($"Dispositivo TWAIN no encontrado: {options.DeviceId}");
            }

            session.TransferMode = _twainConfig.TransferMode;
            session.ShowUI = _twainConfig.ShowUI;
            
            // Configurar resolución
            var resolution = options.DPI > 0 ? 
                Math.Clamp(options.DPI, _twainConfig.MinDPI, _twainConfig.MaxDPI) : 
                _twainConfig.DefaultResolution;
            
            // Configurar capacidades básicas
            if (session.Capabilities.ICapXResolution.IsSupported)
                session.Capabilities.ICapXResolution.SetValue(resolution);
            
            if (session.Capabilities.ICapYResolution.IsSupported)
                session.Capabilities.ICapYResolution.SetValue(resolution);
            
            // Configurar formato de color
            var colorMode = options.ColorMode?.ToLower() switch
            {
                "grayscale" when session.Capabilities.ICapPixelType.IsSupported => PixelType.Gray,
                "blackandwhite" when session.Capabilities.ICapPixelType.IsSupported => PixelType.BlackWhite,
                _ => PixelType.Color
            };
            
            if (session.Capabilities.ICapPixelType.IsSupported)
                session.Capabilities.ICapPixelType.SetValue(colorMode);

            // Configurar dúplex si está soportado
            if (_twainConfig.UseDuplex && session.Capabilities.CapDuplexEnabled.IsSupported)
            {
                session.Capabilities.CapDuplexEnabled.SetValue(true);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al configurar el dispositivo TWAIN");
            throw new ScannerException("Error de configuración del escáner", ex);
        }
    }

    protected void InitializeSession()
    {
        if (_session != null) return;
        
        _session = TWainSession.Create();
        _session.StateChanged += HandleSessionStateChange;
        _session.TransferReady += HandleTransferReady;
        _session.DataTransferred += HandleDataTransferred;
        _session.SourceDisabled += HandleSourceDisabled;
    }

    private void HandleSessionStateChange(object sender, EventArgs e)
    {
        _logger.LogDebug("Estado de sesión TWAIN cambiado: {State}", _session?.State);
    }

    private void HandleTransferReady(object sender, EventArgs e)
    {
        _logger.LogDebug("Transferencia TWAIN lista para iniciar");
    }

    protected virtual void HandleDataTransferred(object sender, DataTransferredEventArgs e)
    {
        if (e.NativeData != null)
        {
            _logger.LogDebug("Datos transferidos: {Size} bytes", e.NativeData.Length);
        }
    }

    private void HandleSourceDisabled(object sender, SourceDisabledEventArgs e)
    {
        _logger.LogDebug("Fuente TWAIN deshabilitada: {Reason}", e.Reason);
        ReleaseResources();
    }

    protected void ReleaseResources()
    {
        try
        {
            if (_session != null)
            {
                if (_session.State > 4) // Si está abierta la fuente
                {
                    _session.CloseSource();
                }
                
                if (_session.State > 3) // Si está abierta la sesión
                {
                    _session.Close();
                }
                
                _session.Dispose();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al liberar recursos TWAIN");
        }
        finally
        {
            _session = null;
        }
    }

    protected async Task<byte[]> ProcessTransferredData(DataTransferredEventArgs e, string targetFormat)
    {
        try
        {
            if (e.NativeData == null)
            {
                throw new ScannerException("No se recibieron datos del escáner");
            }

            using var ms = new MemoryStream();
            await ms.WriteAsync(e.NativeData.ToArray(), 0, e.NativeData.Length);
            
            // Aquí iría la conversión de formato si es necesaria
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar datos transferidos");
            throw new ScannerException("Error al procesar imagen escaneada", ex);
        }
    }

    public abstract Task<ScanResult> ScanAsync(ScanOptions options, IProgress<ScanProgress> progress);
    public abstract Task<DeviceInfo[]> GetDevicesAsync();
    public abstract Task<DeviceCapabilities> GetDeviceCapabilitiesAsync(string deviceId);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                ReleaseResources();
            }
            _disposed = true;
        }
    }

    ~TwainInteropBase()
    {
        Dispose(false);
    }
}
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Services.Interfaces;

/// <summary>
/// Interfaz para el servicio de escaneo que soporta múltiples tecnologías (WIA/TWAIN)
/// </summary>
public interface IScannerService
{
    /// <summary>
    /// Obtiene la lista de dispositivos escáner disponibles
    /// </summary>
    Task<DeviceInfo[]> GetAvailableDevicesAsync();

    /// <summary>
    /// Realiza un escaneo con las opciones especificadas
    /// </summary>
    /// <param name="options">Configuración del escaneo</param>
    /// <param name="sessionId">ID de sesión para seguimiento</param>
    /// <returns>Resultado del escaneo con la imagen y metadatos</returns>
    Task<ScanResult> ScanDocumentAsync(ScanOptions options, string sessionId);

    /// <summary>
    /// Obtiene las capacidades de un dispositivo específico
    /// </summary>
    Task<DeviceCapabilities> GetDeviceCapabilitiesAsync(string deviceId);
}
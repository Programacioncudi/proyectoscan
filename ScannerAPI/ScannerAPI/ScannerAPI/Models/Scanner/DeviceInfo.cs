namespace ScannerAPI.Models.Scanner;

/// <summary>
/// Información de un dispositivo escáner
/// </summary>
public class DeviceInfo
{
    /// <summary>
    /// Identificador único del dispositivo
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Nombre descriptivo del dispositivo
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Indica si el dispositivo está conectado y disponible
    /// </summary>
    public bool IsConnected { get; set; }

    /// <summary>
    /// Tipo de tecnología (WIA, TWAIN, etc.)
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Información adicional del fabricante
    /// </summary>
    public string Manufacturer { get; set; }

    /// <summary>
    /// Modelo específico del dispositivo
    /// </summary>
    public string Model { get; set; }
}
namespace ScannerAPI.Models.Scanner;

/// <summary>
/// Opciones de configuración para un escaneo
/// </summary>
public class ScanOptions
{
    /// <summary>
    /// ID del dispositivo a utilizar
    /// </summary>
    public string DeviceId { get; set; }

    /// <summary>
    /// Resolución en DPI (puntos por pulgada)
    /// </summary>
    public int DPI { get; set; } = 300;

    /// <summary>
    /// Formato de salida (JPEG, PNG, TIFF, PDF)
    /// </summary>
    public string Format { get; set; } = "JPEG";

    /// <summary>
    /// Calidad de compresión (1-100)
    /// </summary>
    public int Quality { get; set; } = 90;

    /// <summary>
    /// Modo de color (Color, Escala de grises, Blanco y negro)
    /// </summary>
    public string ColorMode { get; set; } = "Color";

    /// <summary>
    /// Indica si se debe usar el modo dúplex (escaneo de ambas caras)
    /// </summary>
    public bool UseDuplex { get; set; }

    /// <summary>
    /// Área de escaneo personalizada (en pulgadas)
    /// </summary>
    public ScanArea CustomArea { get; set; }

    /// <summary>
    /// Nombre del perfil de escaneo preconfigurado
    /// </summary>
    public string ProfileName { get; set; }
}

/// <summary>
/// Define un área rectangular para escaneo
/// </summary>
public class ScanArea
{
    public float Left { get; set; }
    public float Top { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
}
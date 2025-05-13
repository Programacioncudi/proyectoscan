// File: Models/Scanner/ScanOptions.cs
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Scanner
{
    /// <summary>
    /// Opciones configurables para realizar un escaneo completo.
    /// </summary>
    public class ScanOptions
    {
        /// <summary>Identificador del dispositivo a utilizar.</summary>
        [Required]
        [MaxLength(100)]
        public string DeviceId { get; set; }

        /// <summary>Resolución en DPI deseada.</summary>
        [Range(1, 10000)]
        public int Dpi { get; set; }

        /// <summary>Modo de color del escaneo.</summary>
        [Required]
        public ColorMode ColorMode { get; set; }

        /// <summary>Profundidad de bits por pixel.</summary>
        [Range(1, 48)]
        public int BitDepth { get; set; }

        /// <summary>Formato de salida del archivo.</summary>
        [Required]
        public FileFormat Format { get; set; }

        /// <summary>Orientación de la página.</summary>
        [Required]
        public Orientation Orientation { get; set; }

        /// <summary>Tamaño de papel estándar.</summary>
        [Required]
        public PaperSize PaperSize { get; set; }

        /// <summary>Calidad de compresión (para JPEG, PNG), de 1 a 100.</summary>
        [Range(1, 100)]
        public int Quality { get; set; } = 90;

        /// <summary>Indica si el escaneo debe ser dúplex (doble cara).</summary>
        public bool Duplex { get; set; }

        /// <summary>Usar alimentador automático de hojas si está disponible.</summary>
        public bool UseFeeder { get; set; }

        /// <summary>Brillo ajustable, rango -100 a 100.</summary>
        [Range(-100, 100)]
        public int Brightness { get; set; }

        /// <summary>Contraste ajustable, rango -100 a 100.</summary>
        [Range(-100, 100)]
        public int Contrast { get; set; }

        /// <summary>Región de recorte en coordenadas de imagen.</summary>
        public CropRegion CropRegion { get; set; }
    }

    /// <summary>
    /// Región rectangular para recortar la imagen escaneada.
    /// </summary>
    public class CropRegion
    {
        [Range(0, int.MaxValue)]
        public int X { get; set; }

        [Range(0, int.MaxValue)]
        public int Y { get; set; }

        [Range(1, int.MaxValue)]
        public int Width { get; set; }

        [Range(1, int.MaxValue)]
        public int Height { get; set; }
    }

    /// <summary>
    /// Modos de color disponibles.
    /// </summary>
    public enum ColorMode
    {
        BlackAndWhite,
        Grayscale,
        Color
    }

    /// <summary>
    /// Formatos de archivo compatibles.
    /// </summary>
    public enum FileFormat
    {
        JPEG,
        PNG,
        PDF,
        TIFF,
        BMP
    }

    /// <summary>
    /// Orientación de la página.
    /// </summary>
    public enum Orientation
    {
        Portrait,
        Landscape
    }

    /// <summary>
    /// Tamaños de papel estándares.
    /// </summary>
    public enum PaperSize
    {
        A4,
        Letter,
        Legal,
        Executive
    }
}


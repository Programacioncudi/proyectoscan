// File: Models/Scanner/DeviceCapabilities.cs
using System.ComponentModel.DataAnnotations;

// File: Models/Scanner/DeviceCapabilities.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Scanner
{
    /// <summary>
    /// Capacidades detalladas de un dispositivo de escaneo.
    /// </summary>
    public class DeviceCapabilities
    {
        /// <summary>Resoluciones soportadas (DPI).</summary>
        [Required]
        public List<int> SupportedDpis { get; set; } = new();

        /// <summary>DPI mínimo soportado.</summary>
        [Range(1, 10000)]
        public int MinDpi { get; set; }

        /// <summary>DPI máximo soportado.</summary>
        [Range(1, 10000)]
        public int MaxDpi { get; set; }

        /// <summary>Modos de color soportados.</summary>
        [Required]
        public List<ColorMode> SupportedColorModes { get; set; } = new();

        /// <summary>Modo de color por defecto.</summary>
        [Required]
        public ColorMode DefaultColorMode { get; set; }

        /// <summary>Formatos de archivo soportados.</summary>
        [Required]
        public List<FileFormat> SupportedFormats { get; set; } = new();

        /// <summary>Profundidad de bits máxima.</summary>
        [Range(1, 48)]
        public int MaxBitDepth { get; set; }

        /// <summary>Indica si soporta dúplex (doble cara).</summary>
        public bool SupportsDuplex { get; set; }

        /// <summary>Indica si tiene alimentador automático de hojas.</summary>
        public bool SupportsFeeder { get; set; }

        /// <summary>Número máximo de hojas en alimentador.</summary>
        [Range(0, 100)]
        public int FeederCapacity { get; set; }
    }

    public enum ColorMode
    {
        BlackAndWhite,
        Grayscale,
        Color
    }

    public enum FileFormat
    {
        JPEG,
        PNG,
        PDF,
        TIFF,
        BMP
    }
}

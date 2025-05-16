// File: Models/Scanner/DeviceInfo.cs
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Scanner
{
    /// <summary>
    /// Información completa de un dispositivo de escaneo.
    /// </summary>
    public class DeviceInfo
    {
        /// <summary>Identificador único del dispositivo.</summary>
        [Required, MaxLength(100)]
        public string Id { get; set; } = string.Empty;

        /// <summary>Nombre descriptivo.</summary>
        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>Fabricante.</summary>
        [MaxLength(100)]
        public string Manufacturer { get; set; } = string.Empty;

        /// <summary>Modelo.</summary>
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;

        /// <summary>Número de serie.</summary>
        [MaxLength(100)]
        public string SerialNumber { get; set; } = string.Empty;

        /// <summary>Capacidades detalladas.</summary>
        [Required]
        public DeviceCapabilities Capabilities { get; set; } = new DeviceCapabilities();
    }
}

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
        public string Id { get; set; }

        /// <summary>Nombre descriptivo.</summary>
        [Required, MaxLength(200)]
        public string Name { get; set; }

        /// <summary>Fabricante.</summary>
        [MaxLength(100)]
        public string Manufacturer { get; set; }

        /// <summary>Modelo.</summary>
        [MaxLength(100)]
        public string Model { get; set; }

        /// <summary>Número de serie.</summary>
        [MaxLength(100)]
        public string SerialNumber { get; set; }

        /// <summary>Capacidades detalladas.</summary>
        [Required]
        public DeviceCapabilities Capabilities { get; set; }
    }
}
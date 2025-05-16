// File: Models/Scanner/ScanOptions.cs
using System;

namespace ScannerAPI.Models.Scanner
{
    /// <summary>
    /// Opciones de escaneo configurables por el usuario.
    /// </summary>
    public class ScanOptions
    {
        /// <summary>
        /// Identificador del dispositivo a usar.
        /// </summary>
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de dispositivo (TWAIN o WIA).
        /// </summary>
        public DeviceType DeviceType { get; set; }

        /// <summary>
        /// Formato de salida.
        /// </summary>
        public FileFormat Format { get; set; }

        /// <summary>
        /// Modo de color.
        /// </summary>
        public ColorMode ColorMode { get; set; }

        /// <summary>
        /// Densidad en DPI.
        /// </summary>
        public int Dpi { get; set; }

        /// <summary>
        /// Cantidad máxima de páginas a escanear.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Tiempo máximo de espera.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Indica si se usa escaneo dúplex.
        /// </summary>
        public bool UseDuplex { get; set; }
    }
}

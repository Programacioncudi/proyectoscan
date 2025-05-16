// File: Models/Scanner/ScanResult.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Scanner
{
    /// <summary>
    /// Resultado final de un escaneo.
    /// </summary>
    public class ScanResult
    {
        /// <summary>Identificador del escaneo.</summary>
        [Required]
        public string ScanId { get; set; } = string.Empty;

        /// <summary>Ruta del archivo generado.</summary>
        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>Indica si fue exitoso.</summary>
        public bool Success { get; set; }

        /// <summary>Mensaje de error en caso de fallo.</summary>
        [MaxLength(1000)]
        public string ErrorMessage { get; set; } = string.Empty;
    }
}

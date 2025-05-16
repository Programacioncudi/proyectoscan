// File: Models/Scanner/ScanProgress.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Scanner
{
    /// <summary>
    /// Estado y progreso de un escaneo en curso.
    /// </summary>
    public class ScanProgress
    {
        /// <summary>Identificador del escaneo.</summary>
        [Required]
        public string ScanId { get; set; } = string.Empty;

        /// <summary>Porcentaje completado (0–100).</summary>
        [Range(0, 100)]
        public int Percentage { get; set; }

        /// <summary>Mensaje descriptivo del paso actual.</summary>
        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        /// <summary>Marca de tiempo UTC de este evento de progreso.</summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}

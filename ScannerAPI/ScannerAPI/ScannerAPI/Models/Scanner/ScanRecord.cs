// File: Models/Scanner/ScanRecord.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Scanner
{
    /// <summary>
    /// Registro de un escaneo realizado.
    /// </summary>
    public class ScanRecord
    {
        /// <summary>Identificador único del registro.</summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>Identificador del escaneo.</summary>
        [Required]
        public string ScanId { get; set; } = string.Empty;

        /// <summary>Ruta local del archivo generado.</summary>
        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>Éxito o fallo del escaneo.</summary>
        [Required]
        public bool Success { get; set; }

        /// <summary>Mensaje de error en caso de fallo.</summary>
        [MaxLength(1000)]
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>Referencia a la sesión de escaneo.</summary>
        public Guid SessionId { get; set; }
    }
}

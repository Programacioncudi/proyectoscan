// File: Models/Scanner/ScanProfile.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Scanner
{
    /// <summary>
    /// Perfil predefinido de escaneo.
    /// </summary>
    public class ScanProfile
    {
        /// <summary>Identificador único del perfil.</summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>Nombre descriptivo del perfil.</summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>Resolución DPI del perfil.</summary>
        [Range(1, 10000)]
        public int Dpi { get; set; }

        /// <summary>Formato de salida.</summary>
        [Required]
        public FileFormat Format { get; set; }

        /// <summary>Calidad de compresión (1-100).</summary>
        [Range(1, 100)]
        public int Quality { get; set; }
    }
}
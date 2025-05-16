// File: Models/Events/ScanProgress.cs
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Events
{
    /// <summary>
    /// Alias para evento de progreso (sinónimo de ProgressEvent).
    /// </summary>
    public class ScanProgress
    {
        /// <summary>Identificador del escaneo.</summary>
        [Required]
        public required Guid ScanId { get; set; } 

        /// <summary>Porcentaje completado (0-100).</summary>
        [Range(0, 100)]
        public int Progress { get; set; }
    }
}


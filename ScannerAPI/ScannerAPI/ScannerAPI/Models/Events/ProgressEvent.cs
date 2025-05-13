// File: Models/Events/ProgressEvent.cs
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Events
{
    /// <summary>
    /// Evento de progreso de escaneo.
    /// </summary>
    public class ProgressEvent
    {
        /// <summary>Identificador del escaneo.</summary>
        [Required]
        public string ScanId { get; set; }

        /// <summary>Porcentaje completado (0-100).</summary>
        [Range(0, 100)]
        public int Percent { get; set; }
    }
}

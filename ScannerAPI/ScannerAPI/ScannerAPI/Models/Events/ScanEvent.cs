// File: Models/Events/ScanEvent.cs
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Events
{
    /// <summary>
    /// Evento de estado de escaneo.
    /// </summary>
    public class ScanEvent
    {
        /// <summary>Identificador del escaneo.</summary>
        [Required]
        public string ScanId { get; set; }

        /// <summary>Estado actual del escaneo.</summary>
        [Required]
        public ScanStatus Status { get; set; }
    }

    /// <summary>
    /// Estados posibles de un escaneo.
    /// </summary>
    public enum ScanStatus
    {
        /// <summary>Escaneo iniciado.</summary>
        Started,
        /// <summary>Escaneo en progreso.</summary>
        InProgress,
        /// <summary>Escaneo completado exitosamente.</summary>
        Completed,
        /// <summary>Escaneo con error.</summary>
        Error
    }
}
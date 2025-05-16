// File: Models/Scanner/ScanSession.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Scanner
{
    /// <summary>
    /// Representa una sesión de escaneo.
    /// </summary>
    public class ScanSession
    {
        /// <summary>Identificador único de la sesión.</summary>
        [Key]
        public Guid SessionId { get; set; }

        /// <summary>Identificador del escaneo asociado.</summary>
        [Required]
        public string ScanId { get; set; } = string.Empty;

        /// <summary>Fecha y hora UTC de inicio.</summary>
        [Required]
        public DateTime StartedAt { get; set; }

        /// <summary>Fecha y hora UTC de finalización.</summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>Identificador del dispositivo utilizado.</summary>
        [Required]
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>Identificador del usuario que inicia la sesión.</summary>
        [Required]
        public Guid UserId { get; set; }
        /// <summary>
        /// Nombre de usuario que inició la sesión.
        /// </summary>
        [Required]
        public string UserName { get; set; } = string.Empty;
        


    }
}

// File: Models/Dtos/ScanSessionDto.cs
using System;

namespace ScannerAPI.Models.Dtos
{
    /// <summary>
    /// DTO para manejar sesiones de escaneo.
    /// </summary>
    public class ScanSessionDto
    {
        /// <summary>
        /// Identificador único de la sesión.
        /// </summary>
        public string SessionId { get; set; } = string.Empty;

        /// <summary>
        /// Identificador del dispositivo asociado a la sesión.
        /// </summary>
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora UTC en que se inició la sesión.
        /// </summary>
        public DateTime StartedAt { get; set; }

        /// <summary>
        /// Fecha y hora UTC en que finalizó la sesión, si aplica.
        /// </summary>
        public DateTime? EndedAt { get; set; }
    }
}

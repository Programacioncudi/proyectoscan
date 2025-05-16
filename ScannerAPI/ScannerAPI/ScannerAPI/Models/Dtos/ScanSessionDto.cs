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
        /// Identificador �nico de la sesi�n.
        /// </summary>
        public string SessionId { get; set; } = string.Empty;

        /// <summary>
        /// Identificador del dispositivo asociado a la sesi�n.
        /// </summary>
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora UTC en que se inici� la sesi�n.
        /// </summary>
        public DateTime StartedAt { get; set; }

        /// <summary>
        /// Fecha y hora UTC en que finaliz� la sesi�n, si aplica.
        /// </summary>
        public DateTime? EndedAt { get; set; }
    }
}

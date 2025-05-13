using System;
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models
{
    /// <summary>
    /// Clase base para todas las entidades con clave primaria.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Identificador único de la entidad.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Fecha y hora de creación UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha y hora de última actualización UTC.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
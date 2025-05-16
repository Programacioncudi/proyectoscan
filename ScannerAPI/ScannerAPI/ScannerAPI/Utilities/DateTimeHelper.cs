// File: Utilities/DateTimeHelper.cs
using System;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Proporciona la hora actual en UTC y hora local, inyectable para facilitar pruebas.
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Obtiene la fecha y hora actual en UTC.
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// Obtiene la fecha y hora actual en la zona horaria local.
        /// </summary>
        DateTime Now { get; }
    }

    /// <summary>
    /// Implementación de <see cref="IDateTimeProvider"/> que usa <see cref="DateTime"/>.
    /// </summary>
    public class DateTimeProvider : IDateTimeProvider
    {
        /// <summary>
        /// Obtiene la fecha y hora actual en UTC (<see cref="DateTime.UtcNow"/>).
        /// </summary>
        public DateTime UtcNow => DateTime.UtcNow;

        /// <summary>
        /// Obtiene la fecha y hora actual en la zona horaria local (<see cref="DateTime.Now"/>).
        /// </summary>
        public DateTime Now => DateTime.Now;
    }
}

// File: Utilities/DateTimeHelper.cs
using System;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Proporciona la hora actual, inyectable para facilitar pruebas.
    /// </summary>
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
        DateTime Now { get; }
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime Now => DateTime.Now;
    }
}
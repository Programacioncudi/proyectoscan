// File: Utilities/StringUtilities.cs
using System;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Funciones de utilidad para manipulación de cadenas.
    /// </summary>
    public static class StringUtilities
    {
        /// <summary>
        /// Comprueba si una cadena es null, vacía o contiene solo espacios.
        /// </summary>
        public static bool IsNullOrWhiteSpace(string value)
            => string.IsNullOrWhiteSpace(value);

        /// <summary>
        /// Trunca la cadena a la longitud especificada, opcionalmente añade sufijo.
        /// </summary>
        public static string Truncate(string value, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
                return value;
            return value.Substring(0, maxLength) + suffix;
        }
    }
}
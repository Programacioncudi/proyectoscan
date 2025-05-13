// File: Utilities/FileNameHelper.cs
using System;
using System.IO;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Ayuda a generar nombres de archivo seguros y únicos.
    /// </summary>
    public static class FileNameHelper
    {
        /// <summary>
        /// Genera un nombre único basado en GUID y extiende la extensión.
        /// </summary>
        public static string GenerateUniqueFileName(string extension)
        {
            var guid = Guid.NewGuid().ToString("N");
            return $"{guid}{extension.StartsWith('.') ? extension : "." + extension}";
        }

        /// <summary>
        /// Sanitiza un nombre de archivo eliminando caracteres inválidos.
        /// </summary>
        public static string Sanitize(string fileName)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(c, '_');
            return fileName;
        }
    }
}

using System;
using System.IO;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Utilidad para generar nombres únicos de archivos escaneados.
    /// </summary>
    public static class DocumentNaming
    {
        /// <summary>
        /// Genera un nombre de archivo único basado en fecha y hora.
        /// </summary>
        public static string Generate(string prefix = "scan", string extension = ".pdf")
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmssfff");
            return $"{prefix}_{timestamp}{extension}";
        }

        /// <summary>
        /// Genera la ruta completa del archivo en un directorio dado.
        /// </summary>
        public static string GenerateFullPath(string directory, string prefix = "scan", string extension = ".pdf")
        {
            var fileName = Generate(prefix, extension);
            return Path.Combine(directory, fileName);
        }
    }
}

using System;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Genera nombres de documentos basados en timestamp y GUID.
    /// </summary>
    public static class DocumentNaming
    {
        /// <summary>
        /// Construye un nombre de archivo �nico anteponiendo el <paramref name="prefix"/>,
        /// seguido de una marca de tiempo UTC y un identificador GUID, m�s la <paramref name="extension"/>.
        /// </summary>
        /// <param name="prefix">Texto inicial para el nombre del documento (por ejemplo, "Invoice").</param>
        /// <param name="extension">Extensi�n del archivo, incluyendo el punto (por ejemplo, ".pdf").</param>
        /// <returns>Nombre de documento �nico en formato "{prefix}_{yyyyMMdd_HHmmss}_{GUID}{extension}".</returns>
        public static string GenerateName(string prefix, string extension)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var id = Guid.NewGuid().ToString("N");
            return $"{prefix}_{timestamp}_{id}{extension}";
        }
    }
}

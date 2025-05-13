using System;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Genera nombres de documentos basados en timestamp y GUID.
    /// </summary>
    public static class DocumentNaming
    {
        public static string GenerateName(string prefix, string extension)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var id = Guid.NewGuid().ToString("N");
            return $"{prefix}_{timestamp}_{id}{extension}";
        }
    }
}
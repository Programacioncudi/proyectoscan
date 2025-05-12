using System.IO;

namespace ScannerAPI.Infrastructure.Storage
{
    /// <summary>
    /// Servicio de almacenamiento local para imágenes y documentos escaneados.
    /// </summary>
    public class LocalScannerStorage
    {
        private readonly string _baseDirectory;

        public LocalScannerStorage(string baseDirectory)
        {
            _baseDirectory = baseDirectory;
            if (!Directory.Exists(_baseDirectory))
                Directory.CreateDirectory(_baseDirectory);
        }

        /// <summary>
        /// Guarda un archivo en el almacenamiento local.
        /// </summary>
        public string SaveFile(string fileName, byte[] data)
        {
            var fullPath = Path.Combine(_baseDirectory, fileName);
            File.WriteAllBytes(fullPath, data);
            return fullPath;
        }

        /// <summary>
        /// Obtiene un archivo del almacenamiento local.
        /// </summary>
        public byte[]? ReadFile(string fileName)
        {
            var fullPath = Path.Combine(_baseDirectory, fileName);
            return File.Exists(fullPath) ? File.ReadAllBytes(fullPath) : null;
        }

        /// <summary>
        /// Elimina un archivo del almacenamiento local.
        /// </summary>
        public bool DeleteFile(string fileName)
        {
            var fullPath = Path.Combine(_baseDirectory, fileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            return false;
        }
    }
}

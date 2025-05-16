// File: Infrastructure/Storage/LocalScannerStorage.cs
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ScannerAPI.Services;

namespace ScannerAPI.Infrastructure.Storage
{
    /// <summary>
    /// Implementación local de <see cref="IStorageService"/>, guarda archivos en disco.
    /// </summary>
    public class LocalScannerStorage : IStorageService
    {
        private readonly string _baseDirectory;
        private readonly ILogger<LocalScannerStorage> _logger;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="LocalScannerStorage"/>.
        /// </summary>
        /// <param name="baseDirectory">Directorio base donde se guardarán los archivos.</param>
        /// <param name="logger">Logger para registrar eventos de almacenamiento.</param>
        /// <exception cref="ArgumentException">Si <paramref name="baseDirectory"/> es null o vacío.</exception>
        public LocalScannerStorage(string baseDirectory, ILogger<LocalScannerStorage> logger)
        {
            if (string.IsNullOrWhiteSpace(baseDirectory))
                throw new ArgumentException("Base directory inválido.", nameof(baseDirectory));

            _baseDirectory = Path.GetFullPath(baseDirectory);
            Directory.CreateDirectory(_baseDirectory);
            _logger = logger;
        }

        /// <summary>
        /// Guarda un archivo en el almacenamiento local.
        /// </summary>
        /// <param name="fileName">Nombre del archivo a guardar.</param>
        /// <param name="data">Contenido en bytes del archivo.</param>
        /// <returns>Ruta completa donde se guardó el archivo.</returns>
        /// <exception cref="IOException">Si ocurre un error al escribir el archivo.</exception>
        public async Task<string> SaveFileAsync(string fileName, byte[] data)
        {
            var safeName = Path.GetFileName(fileName);
            var fullPath = Path.Combine(_baseDirectory, safeName);
            try
            {
                await File.WriteAllBytesAsync(fullPath, data);
                return fullPath;
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "Error guardando archivo {File}", fullPath);
                throw;
            }
        }

        /// <summary>
        /// Lee un archivo del almacenamiento local.
        /// </summary>
        /// <param name="fileName">Nombre del archivo a leer.</param>
        /// <returns>Contenido en bytes del archivo.</returns>
        /// <exception cref="FileNotFoundException">Si el archivo no existe.</exception>
        public async Task<byte[]> ReadFileAsync(string fileName)
        {
            var fullPath = Path.Combine(_baseDirectory, Path.GetFileName(fileName));
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Archivo no encontrado", fullPath);

            return await File.ReadAllBytesAsync(fullPath);
        }

        /// <summary>
        /// Elimina un archivo del almacenamiento local.
        /// </summary>
        /// <param name="fileName">Nombre del archivo a eliminar.</param>
        /// <returns>True si el archivo se eliminó; false si no existía o hubo un error.</returns>
        public Task<bool> DeleteFileAsync(string fileName)
        {
            var fullPath = Path.Combine(_baseDirectory, Path.GetFileName(fileName));
            if (!File.Exists(fullPath))
                return Task.FromResult(false);
            try
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "Error eliminando archivo {File}", fullPath);
                return Task.FromResult(false);
            }
        }
    }
}

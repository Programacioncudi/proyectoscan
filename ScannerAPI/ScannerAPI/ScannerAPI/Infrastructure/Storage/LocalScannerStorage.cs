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

        public LocalScannerStorage(string baseDirectory, ILogger<LocalScannerStorage> logger)
        {
            if (string.IsNullOrWhiteSpace(baseDirectory))
                throw new ArgumentException("Base directory inválido.", nameof(baseDirectory));

            _baseDirectory = Path.GetFullPath(baseDirectory);
            Directory.CreateDirectory(_baseDirectory);
            _logger = logger;
        }

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

        public async Task<byte[]> ReadFileAsync(string fileName)
        {
            var fullPath = Path.Combine(_baseDirectory, Path.GetFileName(fileName));
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Archivo no encontrado", fullPath);

            return await File.ReadAllBytesAsync(fullPath);
        }

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
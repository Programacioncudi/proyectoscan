using System.Threading.Tasks;

namespace ScannerAPI.Services
{
    /// <summary>
    /// Servicio para almacenamiento de archivos de escaneo.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Guarda datos binarios en un archivo y devuelve la ruta completa.
        /// </summary>
        Task<string> SaveFileAsync(string fileName, byte[] data);

        /// <summary>
        /// Lee los datos de un archivo existente.
        /// </summary>
        Task<byte[]> ReadFileAsync(string fileName);

        /// <summary>
        /// Elimina un archivo existente.
        /// </summary>
        Task<bool> DeleteFileAsync(string fileName);
    }
}

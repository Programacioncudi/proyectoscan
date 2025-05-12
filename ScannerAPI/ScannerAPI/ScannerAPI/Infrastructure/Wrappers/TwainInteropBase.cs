using ScannerAPI.Models.Scanner;
using System.Threading.Tasks;

namespace ScannerAPI.Infrastructure.Wrappers
{
    /// <summary>
    /// Clase base abstracta para interoperabilidad con escáneres usando TWAIN.
    /// </summary>
    public abstract class TwainInteropBase : IScannerWrapper
    {
        /// <summary>
        /// Realiza un escaneo usando TWAIN y devuelve el resultado.
        /// </summary>
        /// <param name="options">Opciones del escaneo</param>
        /// <param name="outputPath">Ruta donde guardar el archivo escaneado</param>
        /// <returns>Resultado del escaneo</returns>
        public abstract Task<ScanResult> ScanAsync(ScanOptions options, string outputPath);
    }
}

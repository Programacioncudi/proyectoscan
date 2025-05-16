// File: Services/Interfaces/IScannerService.cs
using ScannerAPI.Models.Scanner;
using System.Threading;
using System.Threading.Tasks;

namespace ScannerAPI.Services.Interfaces
{
    /// <summary>
    /// Define la lógica de negocio de escaneo.
    /// </summary>
    public interface IScannerService
    {
        /// <summary>
        /// Realiza un escaneo y retorna información del resultado.
        /// </summary>
        Task<ScanResult> ScanAsync(ScanOptions options, CancellationToken cancellationToken);

        /// <summary>
        /// Obtiene el resultado de un escaneo previo por su ID.
        /// </summary>
        Task<ScanResult?> GetResultAsync(string scanId);
        /// <summary>
        /// Comprueba si el escáner está disponible para usarse.
        /// </summary>
        /// <param name="cancellationToken">Token que permite cancelar la operación.</param>
        /// <returns><c>true</c> si el escáner está disponible; de lo contrario, <c>false</c>.</returns>
        Task<bool> IsScannerAvailableAsync(CancellationToken cancellationToken);
    }
}

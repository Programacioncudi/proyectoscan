// File: Services/IScannerService.cs
using System.Threading;
using System.Threading.Tasks;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Services
{
    /// <summary>
    /// Lógica de negocio para operaciones de escaneo.
    /// </summary>
    public interface IScannerService
    {
        /// <summary>Ejecuta un escaneo con las opciones dadas.</summary>
        Task<ScanResult> ScanAsync(ScanOptions options, CancellationToken cancellationToken);

        /// <summary>Obtiene el resultado de un escaneo por ID.</summary>
        Task<ScanResult> GetResultAsync(string scanId);
    }
}
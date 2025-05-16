using System.Threading;
using System.Threading.Tasks;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Infrastructure.Wrappers;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Adaptador de <see cref="IScannerWrapper"/> para WIA.
    /// </summary>
    public class WiaWrapper : IScannerWrapper
    {
        private readonly WiaInterop _impl;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="WiaWrapper"/> con la implementación de WIA proporcionada.
        /// </summary>
        /// <param name="impl">Instancia de <see cref="WiaInterop"/> que realiza la comunicación con el escáner.</param>
        public WiaWrapper(WiaInterop impl) => _impl = impl;

        /// <summary>
        /// Determina si las <paramref name="options"/> de escaneo son compatibles con la implementación WIA.
        /// </summary>
        /// <param name="options">Opciones de escaneo a evaluar.</param>
        /// <returns><c>true</c> si la implementación WIA soporta las opciones dadas; de lo contrario, <c>false</c>.</returns>
        public bool Supports(ScanOptions options) => _impl.Supports(options);

        /// <summary>
        /// Ejecuta de forma asíncrona un escaneo utilizando la implementación WIA.
        /// </summary>
        /// <param name="options">Opciones de escaneo a aplicar.</param>
        /// <param name="path">Ruta del archivo donde se guardará la imagen escaneada.</param>
        /// <param name="ct">Token para cancelar la operación.</param>
        /// <returns>Una tarea que al completarse retorna un <see cref="ScanResult"/> con los detalles del escaneo.</returns>
        public Task<ScanResult> ScanAsync(ScanOptions options, string path, CancellationToken ct)
            => _impl.ScanAsync(options, path, ct);
    }
}

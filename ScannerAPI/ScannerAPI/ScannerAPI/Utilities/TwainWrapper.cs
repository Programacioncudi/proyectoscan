using System.Threading;
using System.Threading.Tasks;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Infrastructure.Wrappers;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Adaptador de <see cref="IScannerWrapper"/> para TWAIN.
    /// </summary>
    public class TwainWrapper : IScannerWrapper
    {
        private readonly TwainInteropBase _impl;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="TwainWrapper"/> con la implementaci�n TWAIN proporcionada.
        /// </summary>
        /// <param name="impl">Instancia de <see cref="TwainInteropBase"/> que realiza la comunicaci�n con el esc�ner TWAIN.</param>
        public TwainWrapper(TwainInteropBase impl) => _impl = impl;

        /// <summary>
        /// Determina si las <paramref name="options"/> de escaneo son compatibles con la implementaci�n TWAIN.
        /// </summary>
        /// <param name="options">Opciones de escaneo a evaluar.</param>
        /// <returns><c>true</c> si la implementaci�n TWAIN soporta las opciones dadas; de lo contrario, <c>false</c>.</returns>
        public bool Supports(ScanOptions options) => _impl.Supports(options);

        /// <summary>
        /// Ejecuta de forma as�ncrona un escaneo utilizando la implementaci�n TWAIN.
        /// </summary>
        /// <param name="options">Opciones de escaneo a aplicar.</param>
        /// <param name="path">Ruta del archivo donde se guardar� la imagen escaneada.</param>
        /// <param name="ct">Token para cancelar la operaci�n.</param>
        /// <returns>Una tarea que al completarse retorna un <see cref="ScanResult"/> con los detalles del escaneo.</returns>
        public Task<ScanResult> ScanAsync(ScanOptions options, string path, CancellationToken ct)
            => _impl.ScanAsync(options, path, ct);
    }
}

using Microsoft.Extensions.Logging;
using ScannerAPI.Infrastructure.Wrappers;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using ScannerAPI.Utilities;

namespace ScannerAPI.Infrastructure.Wrappers
{
    /// <summary>
    /// Base abstracto para wrappers TWAIN.
    /// </summary>
    public abstract class TwainInteropBase : ScannerAPI.Services.Interfaces.IScannerWrapper
    {
        /// <summary>
        /// Logger para registrar eventos e informaci�n.
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Utilitario para determinar la arquitectura de proceso (32/64 bits).
        /// </summary>
        protected readonly BitnessHelper _bitnessHelper;

        /// <summary>
        /// Configuraci�n espec�fica de TIFFTWAIN.
        /// </summary>
        protected readonly ITwainConfig _twainConfig;

        /// <summary>
        /// Constructor del base TWAIN interop.
        /// </summary>
        /// <param name="logger">Logger inyectado para registrar informaci�n.</param>
        /// <param name="bitnessHelper">Helper para determinar bitness de la aplicaci�n.</param>
        /// <param name="twainConfig">Configuraci�n espec�fica de TWAIN.</param>
        protected TwainInteropBase(ILogger logger, BitnessHelper bitnessHelper, ITwainConfig twainConfig)
        {
            _logger = logger;
            _bitnessHelper = bitnessHelper;
            _twainConfig = twainConfig;
        }

        /// <inheritdoc />
        public abstract bool Supports(ScanOptions options);

        /// <inheritdoc />
        public abstract Task<ScanResult> ScanAsync(ScanOptions options, string outputPath, CancellationToken cancellationToken);

        /// <summary>
        /// Configura opciones del scanner antes de escanear.
        /// </summary>
        /// <param name="session">Objeto de sesi�n TWAIN.</param>
        /// <param name="options">Opciones de escaneo a configurar.</param>
        protected virtual void ConfigureSource(object session, ScanOptions options)
        {
            // Implementar si es necesario
        }
    }
}


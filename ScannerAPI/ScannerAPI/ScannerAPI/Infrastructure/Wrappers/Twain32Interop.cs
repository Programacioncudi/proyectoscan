// File: Infrastructure/Wrappers/Twain32Interop.cs
// File: Infrastructure/Wrappers/Twain32Interop.cs
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Services.Interfaces;
using ScannerAPI.Utilities;

namespace ScannerAPI.Infrastructure.Wrappers
{
    /// <summary>
    /// Stub de implementación TWAIN de 32-bit.
    /// </summary>
    public class Twain32Interop : TwainInteropBase
    {
        /// <summary>
        /// Crea una instancia de <see cref="Twain32Interop"/>.
        /// </summary>
        public Twain32Interop(
            ILogger<Twain32Interop> logger,
            BitnessHelper bitnessHelper,
            ITwainConfig twainConfig
        ) : base(logger, bitnessHelper, twainConfig)
        {
        }

        /// <inheritdoc />
        public override bool Supports(ScanOptions options)
        {
            // En el stub simplemente validamos que sea TWAIN
            return options.DeviceType == DeviceType.Twain;
        }

        /// <inheritdoc />
        public override async Task<ScanResult> ScanAsync(
            ScanOptions options,
            string outputPath,
            CancellationToken cancellationToken
        )
        {
            // Stub: crea un archivo vacío para simular el escaneo
            await File.WriteAllBytesAsync(outputPath, Array.Empty<byte>(), cancellationToken);
            return new ScanResult
            {
                ScanId = Guid.NewGuid().ToString(),
                FilePath = outputPath,
                Success = true
            };
        }
    }
}

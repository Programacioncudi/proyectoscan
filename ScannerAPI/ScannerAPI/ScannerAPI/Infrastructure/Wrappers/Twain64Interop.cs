
// File: Infrastructure/Wrappers/Twain64Interop.cs

using ScannerAPI.Models.Scanner;
using ScannerAPI.Utilities;

namespace ScannerAPI.Infrastructure.Wrappers
{
    /// <summary>
    /// Stub de implementación de TWAIN 64-bit.
    /// </summary>
    public class Twain64Interop : TwainInteropBase
    {
        /// <summary>
        /// Crea una instancia de <see cref="Twain64Interop"/>.
        /// </summary>
        public Twain64Interop(ILogger<Twain64Interop> logger, BitnessHelper bitnessHelper, ITwainConfig twainConfig)
            : base(logger, bitnessHelper, twainConfig)
        {
        }

        /// <inheritdoc />
        public override bool Supports(ScanOptions options)
        {
            return options.DeviceType == DeviceType.Twain;
        }

        /// <inheritdoc />
        public override async Task<ScanResult> ScanAsync(ScanOptions options, string outputPath, CancellationToken cancellationToken)
        {
            // Stub: crear un archivo vacío como resultado de escaneo
            var bytes = Array.Empty<byte>();
            await File.WriteAllBytesAsync(outputPath, bytes, cancellationToken);
            return new ScanResult
            {
                ScanId = Guid.NewGuid().ToString(),
                FilePath = outputPath,
                Success = true
            };
        }
    }
}

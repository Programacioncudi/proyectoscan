using Microsoft.Extensions.Logging;
using NTwain;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Utilities;
using ScannerAPI.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScannerAPI.Infrastructure.Wrappers
{
    /// <summary>
    /// Implementación de escaneo TWAIN 64-bit usando NTwain.
    /// </summary>
    public class Twain64Interop : TwainInteropBase
    {
        public Twain64Interop(ILogger<Twain64Interop> logger, BitnessHelper bitnessHelper, ITwainConfig twainConfig)
            : base(logger, bitnessHelper, twainConfig)
        {
        }

        public override async Task<ScanResult> ScanAsync(ScanOptions options, string? outputFolder)
        {
            try
            {
                _session = new TWainSession(TWIdentity.CreateFromAssembly(DataGroups.Image, _bitnessHelper.Is64Bit));
                _session.Open();
                ConfigureSource(_session, options);

                var source = _session.GetSources().FirstOrDefault(s => s.ProductName == options.DeviceId);
                if (source == null)
                    throw new Exception($"Escáner TWAIN no encontrado: {options.DeviceId}");

                _session.CurrentSource = source;

                // Aquí se simula la transferencia: en un sistema real se conecta a eventos NTwain.
                string outputPath = Path.Combine(outputFolder ?? Path.GetTempPath(), $"scan_{Guid.NewGuid()}.bmp");
                using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    byte[] dummy = new byte[128];
                    await fs.WriteAsync(dummy);
                }

                return new ScanResult
                {
                    Success = true,
                    OutputPath = outputPath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante escaneo con TWAIN64");
                return new ScanResult { Success = false, ErrorMessage = ex.Message };
            }
            finally
            {
                _session?.Close();
                _session?.Dispose();
            }
        }
    }
}

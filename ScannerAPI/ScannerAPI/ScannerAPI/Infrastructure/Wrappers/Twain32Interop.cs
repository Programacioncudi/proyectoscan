// File: Infrastructure/Wrappers/Twain32Interop.cs
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NTwain;
using NTwain.Data;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Infrastructure.Wrappers
{
    /// <summary>
    /// Implementación TWAIN 32 bits usando NTwain.
    /// </summary>
    public class Twain32Interop : TwainInteropBase
    {
        private readonly TwainSession _session;
        private readonly ILogger<Twain32Interop> _logger;

        public Twain32Interop(ILogger<Twain32Interop> logger)
        {
            _logger = logger;
            _session = new TwainSession(TWIdentity.CreateFromAssembly(DataGroups.Image, Assembly.GetExecutingAssembly()));
            _session.Open();
        }

        public override bool Supports(ScanOptions options)
        {
            return _session.SourceCount > 0;
        }

        public override async Task<ScanResult> ScanAsync(ScanOptions options, string outputPath, CancellationToken cancellationToken = default)
        {
            var source = _session.SourceManager.FirstOrDefault(s => s.Name == options.DeviceId);
            if (source == null)
                throw new InvalidOperationException($"Dispositivo {options.DeviceId} no encontrado.");

            _session.CurrentSource = source;
            source.Capabilities.ICapXResolution.SetCurrentValue(options.Dpi);
            source.Capabilities.ICapYResolution.SetCurrentValue(options.Dpi);
            source.Capabilities.ICapFeederEnabled.SetValue(true, true);
            source.Capabilities.ICapDuplexEnabled.SetValue(options.Duplex, true);

            var buffer = new MemoryStream();
            source.DataTransferred += (s, e) =>
            {
                var bytes = e.GetNativeImageBytes();
                buffer.Write(bytes, 0, bytes.Length);
            };

            source.TransferError += (s, e) =>
            {
                throw new Exception($"Error TWAIN: {e.ErrorCode}");
            };

            source.Acquire();
            // Esperar hasta completar o cancelar
            while (source.State != TwainState.Closed && !cancellationToken.IsCancellationRequested)
                await Task.Delay(100, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException("Escaneo cancelado.");

            buffer.Seek(0, SeekOrigin.Begin);
            await using var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            buffer.WriteTo(fs);

            return new ScanResult
            {
                ScanId = Guid.NewGuid().ToString(),
                FilePath = outputPath,
                Success = true
            };
        }
    }
}

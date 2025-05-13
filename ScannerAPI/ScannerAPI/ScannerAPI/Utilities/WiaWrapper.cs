using System.Threading;
using System.Threading.Tasks;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Infrastructure.Wrappers;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Adaptador de IScannerWrapper para WIA.
    /// </summary>
    public class WiaWrapper : IScannerWrapper
    {
        private readonly WiaInterop _impl;
        public WiaWrapper(WiaInterop impl) => _impl = impl;
        public bool Supports(ScanOptions options) => _impl.Supports(options);
        public Task<ScanResult> ScanAsync(ScanOptions options, string path, CancellationToken ct)
            => _impl.ScanAsync(options, path, ct);
    }
}
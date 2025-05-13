using System.Threading;
using System.Threading.Tasks;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Infrastructure.Wrappers;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Adaptador de IScannerWrapper para TWAIN.
    /// </summary>
    public class TwainWrapper : IScannerWrapper
    {
        private readonly TwainInteropBase _impl;
        public TwainWrapper(TwainInteropBase impl) => _impl = impl;
        public bool Supports(ScanOptions options) => _impl.Supports(options);
        public Task<ScanResult> ScanAsync(ScanOptions options, string path, CancellationToken ct)
            => _impl.ScanAsync(options, path, ct);
    }
}
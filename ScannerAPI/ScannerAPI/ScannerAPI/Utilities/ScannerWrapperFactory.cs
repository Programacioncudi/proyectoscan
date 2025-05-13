using System;
using Microsoft.Extensions.DependencyInjection;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Fabrica para resolver IScannerWrapper con DI.
    /// </summary>
    public static class ScannerWrapperFactory
    {
        public static IScannerWrapper Create(IServiceProvider sp)
        {
            return sp.GetRequiredService<IScannerWrapper>();
        }
    }
}

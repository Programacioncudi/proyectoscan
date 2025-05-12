using Microsoft.Extensions.Logging;
using System;

namespace ScannerAPI
{
    public static class ScannerFactory
    {
        public static IScanner CreateScanner(ScannerType type, ILogger logger = null)
        {
            if (!IsSupported(type))
                throw new ScannerException("Tipo de escáner no soportado en este sistema.");

            try
            {
                return type switch
                {
                    ScannerType.WIA => new ScannerWIA(logger),
                    ScannerType.TWAIN => new ScannerTwain(logger),
                    _ => throw new ScannerException("Tipo de escáner no implementado.")
                };
            }
            catch (DllNotFoundException ex)
            {
                logger?.LogCritical(ex, "Falta biblioteca requerida");
                throw new ScannerException("Controladores no instalados.", ex, ScannerErrorCode.DriverNotFound);
            }
        }

        private static bool IsSupported(ScannerType type)
        {
            if (type == ScannerType.TWAIN && Environment.OSVersion.Platform != PlatformID.Win32NT)
                return false;

            return true;
        }
    }
}
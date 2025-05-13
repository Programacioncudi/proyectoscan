using System;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Ayuda a detectar la arquitectura de proceso y sistema.
    /// </summary>
    public static class BitnessHelper
    {
        public static bool Is64BitProcess => Environment.Is64BitProcess;
        public static bool Is64BitOperatingSystem => Environment.Is64BitOperatingSystem;
    }
}
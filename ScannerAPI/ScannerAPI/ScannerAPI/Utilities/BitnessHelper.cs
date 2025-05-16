// File: Utilities/BitnessHelper.cs
using System;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Provee información sobre la arquitectura (32/64-bit) de la aplicación.
    /// </summary>
    public class BitnessHelper
    {
        /// <summary>
        /// Indica si el proceso está corriendo en 64-bit.
        /// </summary>
        public bool Is64Bit => IntPtr.Size == 8;
    }
}

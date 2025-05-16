// File: Infrastructure/Wrappers/DeviceManager.cs
using System;

namespace ScannerAPI.Infrastructure.Wrappers
{
    /// <summary>
    /// Helper estático para descubrir dispositivos TWAIN/WIA.
    /// </summary>
    public static class DeviceManager
    {
        /// <summary>
        /// Obtiene el escáner predeterminado disponible en el sistema.
        /// </summary>
        /// <returns>Un objeto que representa el escáner predeterminado.</returns>
        public static object GetDefaultScanner()
            => throw new NotImplementedException("Implementa la lógica para obtener el escáner por defecto.");
    }
}

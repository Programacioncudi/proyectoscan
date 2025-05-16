using System;
using Microsoft.Extensions.DependencyInjection;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Fabrica para resolver <see cref="IScannerWrapper"/> usando inyecci�n de dependencias.
    /// </summary>
    public static class ScannerWrapperFactory
    {
        /// <summary>
        /// Obtiene una instancia de <see cref="IScannerWrapper"/> desde el proveedor de servicios.
        /// </summary>
        /// <param name="sp">Proveedor de servicios que contiene la configuraci�n de DI.</param>
        /// <returns>Instancia de <see cref="IScannerWrapper"/> registrada en el contenedor.</returns>
        public static IScannerWrapper Create(IServiceProvider sp)
        {
            return sp.GetRequiredService<IScannerWrapper>();
        }
    }
}


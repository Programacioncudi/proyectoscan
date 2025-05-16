using System;

namespace ScannerAPI.Exceptions
{
    /// <summary>
    /// Excepción de dominio para errores de negocio con código y mensaje.
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Código identificador del error.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="DomainException"/> con un código y mensaje específicos.
        /// </summary>
        /// <param name="code">Código único que identifica el tipo de error de dominio.</param>
        /// <param name="message">Mensaje descriptivo del error.</param>
        public DomainException(string code, string message)
            : base(message)
        {
            Code = code;
        }
    }
}

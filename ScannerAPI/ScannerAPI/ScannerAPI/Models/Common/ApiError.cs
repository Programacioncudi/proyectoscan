// File: Models/Common/ApiError.cs
namespace ScannerAPI.Models.Common
{
    /// <summary>
    /// Representa un error que devuelve la API.
    /// </summary>
    public class ApiError
    {
        /// <summary>
        /// Código único que identifica el error.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Mensaje descriptivo del error.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}

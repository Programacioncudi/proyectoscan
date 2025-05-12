namespace ScannerAPI.Models.Api
{
    /// <summary>
    /// Representa un error detallado retornado por el API.
    /// </summary>
    public class ApiError
    {
        /// <summary>
        /// Código de error único.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Descripción del error para el usuario.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Detalles adicionales útiles para depuración.
        /// </summary>
        public string? Details { get; set; }

        public ApiError(string code, string message, string? details = null)
        {
            Code = code;
            Message = message;
            Details = details;
        }
    }
}

namespace ScannerAPI.Models.Api
{
    /// <summary>
    /// Representa una respuesta genérica del API para compatibilidad con consumidores externos.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Indica si la operación fue exitosa.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mensaje informativo o de error.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Objeto con datos adicionales opcionales.
        /// </summary>
        public object? Data { get; set; }

        public ApiResponse(bool success, string? message = null, object? data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}

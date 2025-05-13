// File: Models/ApiResponse.cs
using System.Text.Json.Serialization;
using ScannerAPI.Models.Api;

namespace ScannerAPI.Models
{
    /// <summary>
    /// Contenedor no genérico para respuestas API.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>Indica si la operación fue exitosa.</summary>
        public bool Success { get; set; }

        /// <summary>Datos devueltos, si los hay.</summary>
        [JsonPropertyName("data")]
        public object Data { get; set; }

        /// <summary>Información de error, si la operación falló.</summary>
        [JsonPropertyName("error")]
        public ApiError Error { get; set; }
    }
}

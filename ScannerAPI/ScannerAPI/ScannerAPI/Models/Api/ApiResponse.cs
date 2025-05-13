// File: Models/Api/ApiResponse.cs
using System.Text.Json.Serialization;

namespace ScannerAPI.Models.Api
{
    /// <summary>
    /// Contenedor genérico para respuestas API.
    /// </summary>
    /// <typeparam name="T">Tipo de datos devueltos.</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>Indica si la operación fue exitosa.</summary>
        public bool Success { get; set; }

        /// <summary>Datos devueltos cuando Success es true.</summary>
        [JsonPropertyName("data")]
        public T Data { get; set; }

        /// <summary>Información de error cuando Success es false.</summary>
        [JsonPropertyName("error")]
        public ApiError Error { get; set; }
    }
}

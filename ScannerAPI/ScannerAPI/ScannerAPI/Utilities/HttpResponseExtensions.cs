using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ScannerAPI.Models.Api;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Extensiones para enviar respuestas HTTP uniformes.
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Serializa la <paramref name="apiResponse"/> a JSON y la escribe en el cuerpo de la respuesta HTTP.
        /// </summary>
        /// <typeparam name="T">Tipo de datos contenido en la respuesta de la API.</typeparam>
        /// <param name="response">Objeto <see cref="HttpResponse"/> en el cual escribir el JSON.</param>
        /// <param name="apiResponse">Objeto <see cref="ApiResponse{T}"/> que será serializado.</param>
        /// <returns>Una tarea que representa la operación de escritura asíncrona.</returns>
        public static async Task WriteJsonAsync<T>(this HttpResponse response, ApiResponse<T> apiResponse)
        {
            response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(apiResponse);
            await response.WriteAsync(json);
        }
    }
}

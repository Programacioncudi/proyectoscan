// File: Utilities/HttpResponseExtensions.cs
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
        public static async Task WriteJsonAsync<T>(this HttpResponse response, ApiResponse<T> apiResponse)
        {
            response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(apiResponse);
            await response.WriteAsync(json);
        }
    }
}
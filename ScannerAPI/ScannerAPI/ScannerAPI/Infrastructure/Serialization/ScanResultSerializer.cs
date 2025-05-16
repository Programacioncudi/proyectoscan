// File: Infrastructure/Serialization/ScanResultSerializer.cs
using System.Text.Json;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Infrastructure.Serialization
{
    /// <summary>
    /// Proporciona métodos para serializar y deserializar <see cref="ScanResult"/>.
    /// </summary>
    public class ScanResultSerializer
    {
        private readonly JsonSerializerOptions _options;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ScanResultSerializer"/>.
        /// </summary>
        /// <param name="options">Opciones para <see cref="JsonSerializer"/>.</param>
        public ScanResultSerializer(JsonSerializerOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Serializa un <see cref="ScanResult"/> a una cadena JSON.
        /// </summary>
        /// <param name="result">El resultado del escaneo a serializar.</param>
        /// <returns>Cadena JSON que representa el <paramref name="result"/>.</returns>
        public string Serialize(ScanResult result)
            => JsonSerializer.Serialize(result, _options);

        /// <summary>
        /// Deserializa una cadena JSON a un <see cref="ScanResult"/>.
        /// </summary>
        /// <param name="json">La cadena JSON que contiene un <see cref="ScanResult"/>.</param>
        /// <returns>Objeto <see cref="ScanResult"/> deserializado.</returns>
        public ScanResult Deserialize(string json)
            => JsonSerializer.Deserialize<ScanResult>(json, _options)!;
    }
}

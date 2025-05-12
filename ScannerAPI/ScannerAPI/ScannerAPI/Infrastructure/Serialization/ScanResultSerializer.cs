using System.Text.Json;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Infrastructure.Serialization
{
    /// <summary>
    /// Serializador y deserializador de resultados de escaneo.
    /// </summary>
    public static class ScanResultSerializer
    {
        /// <summary>
        /// Serializa un resultado de escaneo a una cadena JSON.
        /// </summary>
        public static string Serialize(ScanResult result)
        {
            return JsonSerializer.Serialize(result);
        }

        /// <summary>
        /// Deserializa un JSON a un objeto ScanResult.
        /// </summary>
        public static ScanResult Deserialize(string json)
        {
            return JsonSerializer.Deserialize<ScanResult>(json)!;
        }
    }
}

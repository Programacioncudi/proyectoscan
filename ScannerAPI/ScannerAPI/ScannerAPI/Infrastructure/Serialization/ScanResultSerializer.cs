// File: Infrastructure/Serialization/ScanResultSerializer.cs
using System.Text.Json;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Infrastructure.Serialization
{
    /// <summary>
    /// Serializa y deserializa ScanResult.
    /// </summary>
    public class ScanResultSerializer
    {
        private readonly JsonSerializerOptions _options;

        public ScanResultSerializer(JsonSerializerOptions options)
        {
            _options = options;
        }

        public string Serialize(ScanResult result)
            => JsonSerializer.Serialize(result, _options);

        public ScanResult Deserialize(string json)
            => JsonSerializer.Deserialize<ScanResult>(json, _options)!;
    }
}

    }
}

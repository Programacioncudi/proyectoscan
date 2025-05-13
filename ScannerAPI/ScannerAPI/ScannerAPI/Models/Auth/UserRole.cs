// File: Models/Auth/UserRole.cs
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ScannerAPI.Models.Auth
{
    /// <summary>
    /// Roles disponibles en la aplicación.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRole
    {
        [EnumMember(Value = "ADMIN")]
        Admin,

        [EnumMember(Value = "USER")]
        User
    }
}

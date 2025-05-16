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
        /// <summary>
        /// Administrador con permisos completos.
        /// </summary>
        [EnumMember(Value = "ADMIN")]
        Admin,

        /// <summary>
        /// Usuario estándar con permisos limitados.
        /// </summary>
        [EnumMember(Value = "USER")]
        User
    }
}

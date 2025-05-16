// File: Models/Config/JwtConfig.cs
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Config
{
    /// <summary>
    /// Configuración para tokens JWT.
    /// </summary>
    public class JwtConfig
    {
        /// <summary>Clave secreta usada para firmar tokens.</summary>
        [Required]
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>Emisor del token JWT.</summary>
        [Required]
        public string Issuer { get; set; } = string.Empty;

        /// <summary>Audiencia del token JWT.</summary>
        [Required]
        public string Audience { get; set; } = string.Empty;

        /// <summary>Tiempo de expiración en minutos.</summary>
        [Range(1, 1440)]
        public int ExpiryMinutes { get; set; }
    }
}

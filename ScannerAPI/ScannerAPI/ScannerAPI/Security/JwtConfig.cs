// File: Security/JwtConfig.cs
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Security
{
    /// <summary>
    /// Configuración para creación y validación de tokens JWT.
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// Clave secreta usada para firmar tokens JWT.
        /// </summary>
        [Required]
        public required string SecretKey { get; set; }

        /// <summary>
        /// Emisor (issuer) válido para el token JWT.
        /// </summary>
        [Required]
        public required string Issuer { get; set; }

        /// <summary>
        /// Audiencia (audience) válida para el token JWT.
        /// </summary>
        [Required]
        public required string Audience { get; set; }

        /// <summary>
        /// Duración del token en minutos (1 a 1440).
        /// </summary>
        [Range(1, 1440)]
        public int ExpiryMinutes { get; set; }
    }
}

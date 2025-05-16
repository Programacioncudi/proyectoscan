// File: Security/JwtConfig.cs
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Security
{
    /// <summary>
    /// Configuraci�n para creaci�n y validaci�n de tokens JWT.
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// Clave secreta usada para firmar tokens JWT.
        /// </summary>
        [Required]
        public required string SecretKey { get; set; }

        /// <summary>
        /// Emisor (issuer) v�lido para el token JWT.
        /// </summary>
        [Required]
        public required string Issuer { get; set; }

        /// <summary>
        /// Audiencia (audience) v�lida para el token JWT.
        /// </summary>
        [Required]
        public required string Audience { get; set; }

        /// <summary>
        /// Duraci�n del token en minutos (1 a 1440).
        /// </summary>
        [Range(1, 1440)]
        public int ExpiryMinutes { get; set; }
    }
}
